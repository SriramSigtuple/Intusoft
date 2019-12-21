using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTUSOFT.Configuration.AdvancedSettings
{
    [Serializable]
    public class DoctorSettings
    {
        private static IVLControlProperties doctor_1_name = null;

        public IVLControlProperties Doctor_1_name 
        {
            get 
            { 
                return doctor_1_name; 
            }
            set
            {
                doctor_1_name = value;
            }
        }

        private static IVLControlProperties doctor_1_email = null;

        public IVLControlProperties Doctor_1_email 
        {
            get
            {
                return doctor_1_email;
            }
            set
            {
                doctor_1_email = value;
            }
        }

        private static IVLControlProperties doctor_1_id = null;

        public IVLControlProperties Doctor_1_id 
        {
            get
            {
                return doctor_1_id;
            }
            set
            {
                doctor_1_id = value;
            }
        }


        private static IVLControlProperties doctor_2_name = null;

        public IVLControlProperties Doctor_2_name
        {
            get
            {
                return doctor_2_name;
            }
            set
            {
                doctor_2_name = value;
            }
        }

        private static IVLControlProperties doctor_2_email = null;

        public IVLControlProperties Doctor_2_email
        {
            get
            {
                return doctor_2_email;
            }
            set
            {
                doctor_2_email = value;
            }
        }

        private static IVLControlProperties doctor_2_id = null;

        public IVLControlProperties Doctor_2_id
        {
            get
            {
                return doctor_2_id;
            }
            set
            {
                doctor_2_id = value;
            }
        }

        private static IVLControlProperties doctor_3_name = null;

        public IVLControlProperties Doctor_3_name
        {
            get
            {
                return doctor_3_name;
            }
            set
            {
                doctor_3_name = value;
            }
        }

        private static IVLControlProperties doctor_3_email = null;

        public IVLControlProperties Doctor_3_email
        {
            get
            {
                return doctor_3_email;
            }
            set
            {
                doctor_3_email = value;
            }
        }

        private static IVLControlProperties doctor_3_id = null;

        public IVLControlProperties Doctor_3_id
        {
            get
            {
                return doctor_3_id;
            }
            set
            {
                doctor_3_id = value;
            }
        }

        private static IVLControlProperties doctor_4_name = null;

        public IVLControlProperties Doctor_4_name
        {
            get
            {
                return doctor_4_name;
            }
            set
            {
                doctor_4_name = value;
            }
        }

        private static IVLControlProperties doctor_4_email = null;

        public IVLControlProperties Doctor_4_email
        {
            get
            {
                return doctor_4_email;
            }
            set
            {
                doctor_4_email = value;
            }
        }

        private static IVLControlProperties doctor_4_id = null;

        public IVLControlProperties Doctor_4_id
        {
            get
            {
                return doctor_4_id;
            }
            set
            {
                doctor_4_id = value;
            }
        }

       

        private static IVLControlProperties doctor_5_name = null;

        public IVLControlProperties Doctor_5_name
        {
            get
            {
                return doctor_5_name;
            }
            set
            {
                doctor_5_name = value;
            }
        }

        private static IVLControlProperties doctor_5_email = null;

        public IVLControlProperties Doctor_5_email
        {
            get
            {
                return doctor_5_email;
            }
            set
            {
                doctor_5_email = value;
            }
        }

        private static IVLControlProperties doctor_5_id = null;

        public IVLControlProperties Doctor_5_id
        {
            get
            {
                return doctor_5_id;
            }
            set
            {
                doctor_5_id = value;
            }
        }

        private static IVLControlProperties defaultDoctor = null;

        public IVLControlProperties DefaultDoctor
        {
            get
            {
                return defaultDoctor;
            }
            set
            {
                defaultDoctor = value;
            }
        }

        public DoctorSettings()
        {
            Doctor_1_name = new IVLControlProperties();
            Doctor_1_name.name = "doctor_1_name";
            Doctor_1_name.val = "";
            Doctor_1_name.type = "string";
            Doctor_1_name.control = "System.Windows.Forms.TextBox";
            Doctor_1_name.text = "Doctor1 Name";
            Doctor_1_name.length = 200;

            Doctor_1_email = new IVLControlProperties();
            Doctor_1_email.name = "doctor_1_email";
            Doctor_1_email.val = "";
            Doctor_1_email.type = "string";
            Doctor_1_email.control = "System.Windows.Forms.TextBox";
            Doctor_1_email.text = "Doctor1 Email";
            Doctor_1_email.length = 400;

            Doctor_1_id = new IVLControlProperties();
            Doctor_1_id.name = "doctor_1_id";
            Doctor_1_id.val = "";
            Doctor_1_id.type = "string";
            Doctor_1_id.control = "System.Windows.Forms.TextBox";
            Doctor_1_id.text = "Doctor1 ID";
            Doctor_1_id.length = 200;



            Doctor_2_name = new IVLControlProperties();
            Doctor_2_name.name = "doctor_2_name";
            Doctor_2_name.val = "";
            Doctor_2_name.type = "string";
            Doctor_2_name.control = "System.Windows.Forms.TextBox";
            Doctor_2_name.text = "Doctor2 Name";
            Doctor_2_name.length = 200;

            Doctor_2_email = new IVLControlProperties();
            Doctor_2_email.name = "doctor_2_email";
            Doctor_2_email.val = "";
            Doctor_2_email.type = "string";
            Doctor_2_email.control = "System.Windows.Forms.TextBox";
            Doctor_2_email.text = "Doctor2 Email";
            Doctor_2_email.length = 400;

            Doctor_2_id = new IVLControlProperties();
            Doctor_2_id.name = "doctor_2_id";
            Doctor_2_id.val = "";
            Doctor_2_id.type = "string";
            Doctor_2_id.control = "System.Windows.Forms.TextBox";
            Doctor_2_id.text = "Doctor2 ID";
            Doctor_2_id.length = 200;



            Doctor_3_name = new IVLControlProperties();
            Doctor_3_name.name = "doctor_3_name";
            Doctor_3_name.val = "";
            Doctor_3_name.type = "string";
            Doctor_3_name.control = "System.Windows.Forms.TextBox";
            Doctor_3_name.text = "Doctor3 Name";
            Doctor_3_name.length = 200;

            Doctor_3_email = new IVLControlProperties();
            Doctor_3_email.name = "doctor_3_email";
            Doctor_3_email.val = "";
            Doctor_3_email.type = "string";
            Doctor_3_email.control = "System.Windows.Forms.TextBox";
            Doctor_3_email.text = "Doctor3 Email";
            Doctor_3_email.length = 400;

            Doctor_3_id = new IVLControlProperties();
            Doctor_3_id.name = "doctor_3_id";
            Doctor_3_id.val = "";
            Doctor_3_id.type = "string";
            Doctor_3_id.control = "System.Windows.Forms.TextBox";
            Doctor_3_id.text = "Doctor3 ID";
            Doctor_3_id.length = 200;



            Doctor_4_name = new IVLControlProperties();
            Doctor_4_name.name = "doctor_4_name";
            Doctor_4_name.val = "";
            Doctor_4_name.type = "string";
            Doctor_4_name.control = "System.Windows.Forms.TextBox";
            Doctor_4_name.text = "Doctor4 Name";
            Doctor_4_name.length = 200;

            Doctor_4_email = new IVLControlProperties();
            Doctor_4_email.name = "doctor_4_email";
            Doctor_4_email.val = "";
            Doctor_4_email.type = "string";
            Doctor_4_email.control = "System.Windows.Forms.TextBox";
            Doctor_4_email.text = "Doctor4 Email";
            Doctor_4_email.length = 400;

            Doctor_4_id = new IVLControlProperties();
            Doctor_4_id.name = "doctor_4_id";
            Doctor_4_id.val = "";
            Doctor_4_id.type = "string";
            Doctor_4_id.control = "System.Windows.Forms.TextBox";
            Doctor_4_id.text = "Doctor4 ID";
            Doctor_4_id.length = 200;



            Doctor_5_name = new IVLControlProperties();
            Doctor_5_name.name = "doctor_5_name";
            Doctor_5_name.val = "";
            Doctor_5_name.type = "string";
            Doctor_5_name.control = "System.Windows.Forms.TextBox";
            Doctor_5_name.text = "Doctor5 Name";
            Doctor_5_name.length = 200;

            Doctor_5_email = new IVLControlProperties();
            Doctor_5_email.name = "doctor_5_email";
            Doctor_5_email.val = "";
            Doctor_5_email.type = "string";
            Doctor_5_email.control = "System.Windows.Forms.TextBox";
            Doctor_5_email.text = "Doctor5 Email";
            Doctor_5_email.length = 400;

            Doctor_5_id = new IVLControlProperties();
            Doctor_5_id.name = "doctor_5_id";
            Doctor_5_id.val = "";
            Doctor_5_id.type = "string";
            Doctor_5_id.control = "System.Windows.Forms.TextBox";
            Doctor_5_id.text = "Doctor5 ID";
            Doctor_5_id.length = 200;



            DefaultDoctor = new IVLControlProperties();
            DefaultDoctor.name = "defaultDoctor";
            DefaultDoctor.val = "1";
            DefaultDoctor.type = "string";
            DefaultDoctor.control = "System.Windows.Forms.ComboBox";
            DefaultDoctor.range = "1,2,3,4,5";
            DefaultDoctor.text = "Default Doctor";
            DefaultDoctor.length = 200;
        }
    }
}
