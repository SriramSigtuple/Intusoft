using Cloud_Models.Models;
using System;

namespace IVLUploaderTestProject
{
    public class URL_ComputeHelperTestClassData
    {
        public static Object[] uRL_Model =
        {
            new Object[]{ new URL_Model {API_URL= "TestData/",
                API_URL_End_Point = "TestData",
                API_URL_Mid_Point = "TestData",
                API_URL_Start_Point = "TestData" },
                @"TestData/TestData/TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "TestData/",
                API_URL_End_Point = "",
                API_URL_Mid_Point = "TestData",
                API_URL_Start_Point = "TestData" },
                @"TestData/TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "TestData/",
                API_URL_End_Point = "TestData",
                API_URL_Mid_Point = "",
                API_URL_Start_Point = "TestData" },
                @"TestData/TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "TestData/",
                API_URL_End_Point = "TestData",
                API_URL_Mid_Point = "TestData",
                API_URL_Start_Point = "" },
                @"TestData/TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "",
                API_URL_End_Point = "TestData",
                API_URL_Mid_Point = "TestData",
                API_URL_Start_Point = "TestData" },
                @"TestData/TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "TestData/",
                API_URL_End_Point = "",
                API_URL_Mid_Point = "",
                API_URL_Start_Point = "TestData" },
                @"TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "TestData/",
                API_URL_End_Point = "TestData",
                API_URL_Mid_Point = "",
                API_URL_Start_Point = "" },
                @"TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "",
                API_URL_End_Point = "",
                API_URL_Mid_Point = "TestData",
                API_URL_Start_Point = "TestData" },
                @"TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "TestData/TestData/TestData/TestData",
                API_URL_End_Point = "",
                API_URL_Mid_Point = "",
                API_URL_Start_Point = "" },
                @"TestData/TestData/TestData/TestData" },

            new Object[]{new URL_Model{API_URL= "TestData",
                API_URL_End_Point = "TestData",
                API_URL_Mid_Point = "TestData",
                API_URL_Start_Point = "TestData" },
                @"TestData/TestData/TestData/TestData" },

           
        };
        

    }
}
