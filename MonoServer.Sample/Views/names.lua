{(header.lua)}
<h2>Names</h2>
<ul>
{%
for name in Each(people) do
%}
<li>{{ name }}</li>
{% end %}
</ul>
{(footer.lua)}