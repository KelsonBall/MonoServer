local setmetatable = setmetatable
local loadstring = loadstring
local loadchunk
local tostring = tostring
local setfenv = setfenv
local require = require
local capture
local concat = table.concat
local assert = assert
local prefix
local write = io.write
local pcall = pcall
local phase
local open = io.open
local load = load
local type = type
local dump = string.dump
local find = string.find
local gsub = string.gsub
local byte = string.byte
local null
local sub = string.sub
local ngx = ngx
local jit = jit
local var

local _VERSION = _VERSION
local _ENV = _ENV
local _G = _G

local HTML_ENTITIES = {
    ["&"] = "&amp;",
    ["<"] = "&lt;",
    [">"] = "&gt;",
    ['"'] = "&quot;",
    ["'"] = "&#39;",
    ["/"] = "&#47;"
}

local CODE_ENTITIES = {
    ["{"] = "&#123;",
    ["}"] = "&#125;",
    ["&"] = "&amp;",
    ["<"] = "&lt;",
    [">"] = "&gt;",
    ['"'] = "&quot;",
    ["'"] = "&#39;",
    ["/"] = "&#47;"
}

local ok, newtab = pcall(require, "table.new")
if not ok then newtab = function() return {} end end

local template = newtab(0, 12)

template._VERSION = "1.9"

local function enabled(val)
    if val == nil then return true end
    return val == true or (val == "1" or val == "true" or val == "on")
end

local function trim(s)
    return gsub(gsub(s, "^%s+", ""), "%s+$", "")
end

local function rpos(view, s)
    while s > 0 do
        local c = sub(view, s, s)
        if c == " " or c == "\t" or c == "\0" or c == "\x0B" then
            s = s - 1
        else
            break
        end
    end
    return s
end

local function escaped(view, s)
    if s > 1 and sub(view, s - 1, s - 1) == "\\" then
        if s > 2 and sub(view, s - 2, s - 2) == "\\" then
            return false, 1
        else
            return true, 1
        end
    end
    return false, 0
end

do    
    template.print = write    
    
    local context = { __index = function(t, k)
        return t.context[k] or t.template[k] or _G[k]
    end }
    if jit then
        loadchunk = function(view)
            return assert(load(view, nil, nil, setmetatable({ template = template }, context)))
        end
    else
        loadchunk = function(view)
            local func = assert(loadstring(view))
            setfenv(func, setmetatable({ template = template }, context))
            return func
        end
    end    
end

function template.output(s)
    if s == nil or s == null then return "" end
    if type(s) == "function" then return template.output(s()) end
    return tostring(s)
end

function template.escape(s, c)
    if type(s) == "string" then
        if c then return gsub(s, "[}{\">/<'&]", CODE_ENTITIES) end
        return gsub(s, "[\">/<'&]", HTML_ENTITIES)
    end
    return template.output(s)
end

function template.new(view, map)        
    return setmetatable({ }, { __tostring = function(self) return template.compile(view, map)(self) end })
end

function template.compile(view, map)
    view = map[view]
    local func = loadchunk(template.parse(view))
    return func, false
end

function template.parse(view)
    local j = 2
    local c = {[[
context=... or {}
local function include(v, c) return template.compile(v)(c or context) end
local ___,blocks,layout={},blocks or {}
]] }
    local i, s = 1, find(view, "{", 1, true)
    while s do
        local t, p = sub(view, s + 1, s + 1), s + 2
        if t == "{" then
            local e = find(view, "}}", p, true)
            if e then
                local z, w = escaped(view, s)
                if i < s - w then
                    c[j] = "___[#___+1]=[=[\n"
                    c[j+1] = sub(view, i, s - 1 - w)
                    c[j+2] = "]=]\n"
                    j=j+3
                end
                if z then
                    i = s
                else
                    c[j] = "___[#___+1]=template.escape("
                    c[j+1] = trim(sub(view, p, e - 1))
                    c[j+2] = ")\n"
                    j=j+3
                    s, i = e + 1, e + 2
                end
            end
        elseif t == "*" then
            local e = find(view, "*}", p, true)
            if e then
                local z, w = escaped(view, s)
                if i < s - w then
                    c[j] = "___[#___+1]=[=[\n"
                    c[j+1] = sub(view, i, s - 1 - w)
                    c[j+2] = "]=]\n"
                    j=j+3
                end
                if z then
                    i = s
                else
                    c[j] = "___[#___+1]=template.output("
                    c[j+1] = trim(sub(view, p, e - 1))
                    c[j+2] = ")\n"
                    j=j+3
                    s, i = e + 1, e + 2
                end
            end
        elseif t == "%" then
            local e = find(view, "%}", p, true)
            if e then
                local z, w = escaped(view, s)
                if z then
                    if i < s - w then
                        c[j] = "___[#___+1]=[=[\n"
                        c[j+1] = sub(view, i, s - 1 - w)
                        c[j+2] = "]=]\n"
                        j=j+3
                    end
                    i = s
                else
                    local n = e + 2
                    if sub(view, n, n) == "\n" then
                        n = n + 1
                    end
                    local r = rpos(view, s - 1)
                    if i <= r then
                        c[j] = "___[#___+1]=[=[\n"
                        c[j+1] = sub(view, i, r)
                        c[j+2] = "]=]\n"
                        j=j+3
                    end
                    c[j] = trim(sub(view, p, e - 1))
                    c[j+1] = "\n"
                    j=j+2
                    s, i = n - 1, n
                end
            end
        elseif t == "(" then
            local e = find(view, ")}", p, true)
            if e then
                local z, w = escaped(view, s)
                if i < s - w then
                    c[j] = "___[#___+1]=[=[\n"
                    c[j+1] = sub(view, i, s - 1 - w)
                    c[j+2] = "]=]\n"
                    j=j+3
                end
                if z then
                    i = s
                else
                    local f = sub(view, p, e - 1)
                    local x = find(f, ",", 2, true)
                    if x then
                        c[j] = "___[#___+1]=include([=["
                        c[j+1] = trim(sub(f, 1, x - 1))
                        c[j+2] = "]=],"
                        c[j+3] = trim(sub(f, x + 1))
                        c[j+4] = ")\n"
                        j=j+5
                    else
                        c[j] = "___[#___+1]=include([=["
                        c[j+1] = trim(f)
                        c[j+2] = "]=])\n"
                        j=j+3
                    end
                    s, i = e + 1, e + 2
                end
            end
        elseif t == "[" then
            local e = find(view, "]}", p, true)
            if e then
                local z, w = escaped(view, s)
                if i < s - w then
                    c[j] = "___[#___+1]=[=[\n"
                    c[j+1] = sub(view, i, s - 1 - w)
                    c[j+2] = "]=]\n"
                    j=j+3
                end
                if z then
                    i = s
                else
                    c[j] = "___[#___+1]=include("
                    c[j+1] = trim(sub(view, p, e - 1))
                    c[j+2] = ")\n"
                    j=j+3
                    s, i = e + 1, e + 2
                end
            end
        elseif t == "-" then
            local e = find(view, "-}", p, true)
            if e then
                local x, y = find(view, sub(view, s, e + 1), e + 2, true)
                if x then
                    local z, w = escaped(view, s)
                    if z then
                        if i < s - w then
                            c[j] = "___[#___+1]=[=[\n"
                            c[j+1] = sub(view, i, s - 1 - w)
                            c[j+2] = "]=]\n"
                            j=j+3
                        end
                        i = s
                    else
                        y = y + 1
                        x = x - 1
                        if sub(view, y, y) == "\n" then
                            y = y + 1
                        end
                        local b = trim(sub(view, p, e - 1))
                        if b == "verbatim" or b == "raw" then
                            if i < s - w then
                                c[j] = "___[#___+1]=[=[\n"
                                c[j+1] = sub(view, i, s - 1 - w)
                                c[j+2] = "]=]\n"
                                j=j+3
                            end
                            c[j] = "___[#___+1]=[=["
                            c[j+1] = sub(view, e + 2, x)
                            c[j+2] = "]=]\n"
                            j=j+3
                        else
                            if sub(view, x, x) == "\n" then
                                x = x - 1
                            end
                            local r = rpos(view, s - 1)
                            if i <= r then
                                c[j] = "___[#___+1]=[=[\n"
                                c[j+1] = sub(view, i, r)
                                c[j+2] = "]=]\n"
                                j=j+3
                            end
                            c[j] = 'blocks["'
                            c[j+1] = b
                            c[j+2] = '"]=include[=['
                            c[j+3] = sub(view, e + 2, x)
                            c[j+4] = "]=]\n"
                            j=j+5
                        end
                        s, i = y - 1, y
                    end
                end
            end
        elseif t == "#" then
            local e = find(view, "#}", p, true)
            if e then
                local z, w = escaped(view, s)
                if i < s - w then
                    c[j] = "___[#___+1]=[=[\n"
                    c[j+1] = sub(view, i, s - 1 - w)
                    c[j+2] = "]=]\n"
                    j=j+3
                end
                if z then
                    i = s
                else
                    e = e + 2
                    if sub(view, e, e) == "\n" then
                        e = e + 1
                    end
                    s, i = e - 1, e
                end
            end
        end
        s = find(view, "{", s + 1, true)
    end
    s = sub(view, i)
    if s and s ~= "" then
        c[j] = "___[#___+1]=[=[\n"
        c[j+1] = s
        c[j+2] = "]=]\n"
        j=j+3
    end
    c[j] = "return layout and include(layout,setmetatable({view=table.concat(___),blocks=blocks},{__index=context})) or table.concat(___)"
    return concat(c)
end

return template