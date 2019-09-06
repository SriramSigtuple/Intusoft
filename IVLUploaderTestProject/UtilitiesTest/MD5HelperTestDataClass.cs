using Common;
using System;
using System.IO;

namespace IVLUploaderTestProject
{
    public class MD5HelperTestDataClass
    {
        public static Object[] stringTestDataArrays =
        {
            new object[]{ "007e0830793f57812799933a6d3c470b", "20190823112930.jpg"},
            new object[]{ "a8871f5303a4d77341c371f4cfd29497", "20190827115035.jpg"},
            new object[]{ "0f7d223aec9205349973cbee4bbf4439", "20190827121218.jpg"},
            new object[]{ "5ddc2eca7f2966a120c2edf479b7a844", "20190827121557.jpg"},
            new object[]{ "a77ec26479cc785ed79bd75a7a599bb5", "20190827121721.jpg"},
            new object[]{ "2eff937a9eba4de280ed421d98001a40", "20190723130616.png"},
            new object[]{ "8b5cfc4c1e0c512f5d4c961fa9362bfc", "20190723123128.png"},
            new object[]{ "fb9b6ed6b0416fcee089e6b220a99a08", "20190723123002.png"},
            new object[]{ "85c6f0b8c0af8ea0790b128699e3c6c2", "20190723122730.png"},
            new object[]{ "a0e618bf8d22ab04c2fc74fba74ea93a", "20190722172230.png"},
        };

        public static Object[] fileInfoWrongTestDataArrays =
        {
            
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190823112930.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190827115035.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190827121218.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190827121557.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190827121721.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190723130616.png")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190723123128.png")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190723123002.png")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190723122730.png")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo(@"C:\IVLImageRepo\Images\20190722172230.png")},
        };

        public static Object[] fileInfoCorrectTestDataArrays =
        {
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190823112930.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190827115035.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190827121218.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190827121557.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190827121721.jpg")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190723130616.png")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190723123128.png")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190723123002.png")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo( @"C:\IVLImageRepo\Images\20190723122730.png")},
            new object[]{ new MD5ReturnResponse() ,new FileInfo(@"C:\IVLImageRepo\Images\20190722172230.png")},
        };
    }
}
