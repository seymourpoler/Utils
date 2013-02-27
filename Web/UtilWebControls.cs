public class Util
{
    public static int? ToNullInt(string t)
    {
        int i = 0;
        if (int.TryParse(t, out i))
        {
            return i;
        }
        else
        {
            return null;
        }
    }
    public static short? ToNullShort(string t)
    {
        short i = 0;
        if (short.TryParse(t, out i))
        {
            return i;
        }
        else
        {
            return null;
        }
    }
    public static DateTime? ToNullDate(string t)
    {
        DateTime d = new DateTime();
        if (DateTime.TryParse(t, out d))
        {
            return d;
        }
        else
        {
            return null;
        }
    }
    public static decimal? ToNullDecimal(string t)
    {
        decimal d = 0;
        if (decimal.TryParse(t, out d))
        {
            return d;
        }
        else
        {
            return null;
        }
    }
    public static byte? ToNullByte(string t)
    {
        byte d = 0;
        if (byte.TryParse(t, out d))
        {
            return d;
        }
        else
        {
            return null;
        }
    }
    public static bool? ToNullBool(string t)
    {
        bool d = false;
        if (bool.TryParse(t, out d))
        {
            return d;
        }
        else
        {
            return null;
        }
    }
    public static string ToNullString(string t)
    {
        if (!string.IsNullOrEmpty(t))
        {
            return t;
        }
        else
        {
            return null;
        }
    }
    public static object ToNullValue(PropertyInfo propiedadDestino, PropertyInfo propiedadOrigen, Object objeto)
    {
        if (propiedadDestino.PropertyType == typeof(DateTime)
        || propiedadDestino.PropertyType == typeof(DateTime?))
        {
            return ToNullDate(propiedadOrigen.GetValue(objeto, null).ToString());
        }
        else if (propiedadDestino.PropertyType == typeof(int)
        || propiedadDestino.PropertyType == typeof(int?))
        {
            return ToNullInt(propiedadOrigen.GetValue(objeto, null).ToString());
        }
        else if (propiedadDestino.PropertyType == typeof(Int16)
        || propiedadDestino.PropertyType == typeof(Int16?))
        {
            return ToNullShort(propiedadOrigen.GetValue(objeto, null).ToString());
        }
        else if (propiedadDestino.PropertyType == typeof(decimal)
        || propiedadDestino.PropertyType == typeof(decimal?))
        {
            return ToNullDecimal(propiedadOrigen.GetValue(objeto, null).ToString());
        }
        else if (propiedadDestino.PropertyType == typeof(byte)
        || propiedadDestino.PropertyType == typeof(byte?))
        {
            return ToNullByte(propiedadOrigen.GetValue(objeto, null).ToString());
        }
        else if (propiedadDestino.PropertyType == typeof(bool)
        || propiedadDestino.PropertyType == typeof(bool?))
        {
            return ToNullBool(propiedadOrigen.GetValue(objeto, null).ToString());
        }
        else if (propiedadDestino.PropertyType == typeof(string))
        {
            object value = propiedadOrigen.GetValue(objeto, null);
            if (((string)value) == "") value = null;
            return value;
        }
        else
        {
            return propiedadOrigen.GetValue(objeto, null);
        }
    }
    public static Control GetControl(string id, Control contenedor)
    {
        Control control = contenedor.FindControl(id);
        if (control != null) return control;
        if (contenedor.Controls.Count != 0)
        {
            foreach (Control c in contenedor.Controls)
            {
                control = GetControl(id, c);
                if (control != null) return control;
            }
        }
        return null;
    }
    public static object FillObject(Control controlPadre, Type type, string prefijoIdControl)
    {
        //Crea un objeto del tipo dado
        object entity = Activator.CreateInstance(type);

        // Para cada propiedad del objeto
        foreach (PropertyInfo propiedad in type.GetProperties())
        {
            if (propiedad.CanWrite)
            {
                //Busca un control con id igual a "rep_" mas el nombre de la propiedad
                string idControl = string.Format("{0}{1}", prefijoIdControl, propiedad.Name);
                Control control = GetControl(idControl, controlPadre);

                //Si el control existe
                if (control != null)
                {
                    // Segun el tipo del control necesitamos una propiedad concreta
                    string nombrePropieda = "";
                    if (control is Ext.Net.CheckColumn) nombrePropieda = "Checked";
                    else if (control is Ext.Net.Hidden) nombrePropieda = "Value";
                    else if (control is Ext.Net.ComboBox) nombrePropieda = "SelectedValue";
                    else nombrePropieda = "Text";

                    // Recuperamos la propiedad del control que vamos a leer
                    PropertyInfo propiedadControl = control.GetType().GetProperty(nombrePropieda);

                    // Si la propiedad esta disponible
                    if (propiedadControl != null && propiedadControl.CanRead)
                    {
                        propiedad.SetValue(entity, ToNullValue(propiedad, propiedadControl, control), null);
                    }
                }
            }
        }
        return entity;
    }
    public static bool IsNullObject(object obj)
    {
        object propiedadControl;
        bool resultado = true;

        // Para cada propiedad del objeto
        foreach (PropertyInfo propiedad in obj.GetType().GetProperties())
        {
            if ((null != propiedad) && (propiedad.CanRead))
            {
                // Recuperamos la propiedad del control que vamos a leer
                propiedadControl = propiedad.GetValue(obj, null);
                if ((null != propiedadControl) &&
                    (false == propiedadControl.ToString().Contains("List"))) //No se toma en cuenta las listas --> IList
                {
                    resultado = false;
                    break;
                }

            }
        }
        return resultado;
    }
    public static void FillControl<t>(Control controlPadre, t entity)
    {
        FillControl(controlPadre, entity, "");
    }
    public static void FillControl<t>(Control controlPadre, t entity, string prefijoIdControl)
    {

        // Para cada propiedad del objeto
        foreach (PropertyInfo propiedad in entity.GetType().GetProperties())
        {
            if (propiedad.CanRead)
            {
                //Busca un control con id igual a "rep_" mas el nombre de la propiedad
                string idControl = string.Format("{0}{1}", prefijoIdControl, propiedad.Name);
                Control control = GetControl(idControl, controlPadre);

                //Si el control existe
                if (control != null)
                {
                    // Segun el tipo del control necesitamos una propiedad concreta
                    string nombrePropieda = "";
                    if (control is CheckColumn) nombrePropieda = "Checked";
                    else if (control is Hidden) nombrePropieda = "Value";
                    else if (control is ComboBox) nombrePropieda = "SelectedValue";
                    else if (control is Image) nombrePropieda = "ImageUrl";
                    else nombrePropieda = "Text";

                    // Recuperamos la propiedad del control que vamos a leer
                    PropertyInfo propiedadControl = control.GetType().GetProperty(nombrePropieda);

                    // Si la propiedad esta disponible
                    if (propiedadControl != null)
                    {
                        object value = propiedad.GetValue(entity, null);
                        //porque el setvalue, da problemas con el tipo boolean
                        if ((value != null) && (value.GetType() != typeof(Boolean)))
                            propiedadControl.SetValue(control, value.ToString(), null);
                        else
                            propiedadControl.SetValue(control, value, null);
                    }
                }
            }
        }
    }
    public static void ClearControl<t>(Control controlPadre, t entity, string prefijoIdControl)
    {
        // Para cada propiedad del objeto
        foreach (PropertyInfo propiedad in entity.GetType().GetProperties())
        {
            if (propiedad.CanRead)
            {
                //Busca un control con id igual a "rep_" mas el nombre de la propiedad
                string idControl = string.Format("{0}{1}", prefijoIdControl, propiedad.Name);
                Control control = GetControl(idControl, controlPadre);

                //Si el control existe
                if (control != null)
                {
                    // Segun el tipo del control necesitamos una propiedad concreta
                    string nombrePropieda = "";
                    if (control is CheckColumn) nombrePropieda = "Checked";
                    else if (control is Hidden) nombrePropieda = "Value";
                    else if (control is ComboBox) nombrePropieda = "SelectedValue";
                    else if (control is Image) nombrePropieda = "ImageUrl";
                    else nombrePropieda = "Text";

                    // Recuperamos la propiedad del control que vamos a leer
                    PropertyInfo propiedadControl = control.GetType().GetProperty(nombrePropieda);

                    // Si la propiedad esta disponible
                    if (propiedadControl != null)
                    {
                        propiedadControl.SetValue(control, null, null);
                    }
                }
            }
        }
    }
}