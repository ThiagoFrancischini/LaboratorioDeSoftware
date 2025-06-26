namespace LaboratorioDeSoftware.Tools;

public static class SessionTools {
    public static bool IsUsuarioAdmin(HttpContext context)
    {
        if (context.Session.GetString("UsuarioId") == null || context.Session.GetInt32("TipoUsuario") == null || context.Session.GetInt32("TipoUsuario") != Convert.ToInt32(Core.Entities.Enums.Enums.enTipoUsuario.Administrador))
        {
            return false;
        }

        return true;
    }

    public static Guid GetUserLogadoId(HttpContext context)
    {
        if(context == null || context.Session.GetString("UsuarioId") == null)
        {
            return Guid.Empty;
        }

        return Guid.Parse(context.Session.GetString("UsuarioId"));
    }
}