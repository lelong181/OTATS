using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHang
/// </summary>

[Serializable]
public class Quyen
{
    public Quyen()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private int id_Quyen;

    public int ID_Quyen
    {
        get { return id_Quyen; }
        set { id_Quyen = value; }
    }

    private string tenQuyen;

    public string TenQuyen
    {
        get { return tenQuyen; }
        set { tenQuyen = value; }
    }
}