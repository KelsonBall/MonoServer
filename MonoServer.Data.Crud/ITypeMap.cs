using System.Reflection;

namespace MonoServer.Data.Crud
{
	public interface ITypeMap
	{
		string Get(PropertyInfo property);
	}
}
