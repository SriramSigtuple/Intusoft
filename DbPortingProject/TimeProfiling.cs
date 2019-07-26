using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Common;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Repository;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NLog.Targets;

namespace DBPorting
{
    public enum ModelsEnum
    {
        ModelNotSelected=1,
        Patient,
        Visits,
        Images,
        Reports,
        Annotation
    }

    public enum MethodEnum
    {
        MethodNotSelected=1,
        GetById,
        GetAll,
        GetPageData
    }
    
    public partial class TimeProfiling : Form
    {
        public Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");
        Type type;
        public TimeProfiling()
        {
            InitializeComponent();
            type = null;
            ModelsEnum[] Model_values = Enum.GetValues(typeof(ModelsEnum)) as ModelsEnum[];
            string[] Model_description_values = Model_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < Model_values.Length; i++)
            {
                if (Model_description_values[i].Contains('_'))
                    Model_description_values[i] = INTUSOFT.Data.Common.GetDescription(Model_values[i]);
            }
            model_cbx.DataSource = Model_description_values;
            MethodEnum[] Method_values = Enum.GetValues(typeof(MethodEnum)) as MethodEnum[];
            string[] Method_description_values = Method_values.OfType<object>().Select(o => o.ToString()).ToArray();
            for (int i = 0; i < Method_values.Length; i++)
            {
                if (Method_description_values[i].Contains('_'))
                    Method_description_values[i] = INTUSOFT.Data.Common.GetDescription(Method_values[i]);
            }
            method_cbx.DataSource = Method_description_values;
        }

        public void ModelSelection()
        {
            try
            {
                switch ((ModelsEnum)Enum.Parse(typeof(ModelsEnum), model_cbx.SelectedItem.ToString(), true))
                {
                    case ModelsEnum.Patient:
                        MethodSelection<Patient>();
                        break;
                    case ModelsEnum.Visits:
                        MethodSelection<visit>();
                        break;
                    case ModelsEnum.Images:
                        MethodSelection<obs>();
                        break;
                    case ModelsEnum.Reports:
                        MethodSelection<report>();
                        break;
                    case ModelsEnum.Annotation:
                        MethodSelection<eye_fundus_image_annotation>();
                        break;
                    case ModelsEnum.ModelNotSelected:
                        MessageBox.Show("No valid model type selected");
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        public void MethodSelection<T>() where T : class,IBaseModel 
        {
            try
            {
                switch ((MethodEnum)Enum.Parse(typeof(MethodEnum), method_cbx.SelectedItem.ToString(), true))
                {
                    case MethodEnum.GetAll:
                        List<T> allDataList = NewDataVariables._Repo.GetAll<T>().ToList<T>();
                        break;
                    case MethodEnum.GetById:
                        Type type = typeof(T);
                        if (type == typeof(Patient) || type == typeof(visit))
                        {
                            T singleData = NewDataVariables._Repo.GetById<T>(1);
                        }
                        else
                        {
                            visit visits = visit.CreateNewVisit();
                            visits.visitId = 2;
                            List<T> getbycat = NewDataVariables._Repo.GetByCategory<T>("visit", visits).ToList();
                        }
                        break;
                    case MethodEnum.GetPageData:
                        List<T> allDataPage = NewDataVariables._Repo.GetPageData<T>(10, 1).ToList<T>();
                        break;
                    case MethodEnum.MethodNotSelected:
                        MessageBox.Show("No method type selected");
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void timechk_btn_Click(object sender, EventArgs e)
        {
            ModelSelection();
        }
    }
}
