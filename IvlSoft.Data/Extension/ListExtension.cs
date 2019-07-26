using System;
using INTUSOFT.Data.NewDbModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace INTUSOFT.Data.Extension
{
    public static class ListExtension
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            Double d = 1.0D;
            d.ToString();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.Name != "Visits" && prop.Name != "Images")
                table.Columns.Add(prop.Name, typeof(String));// Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);                
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    if(prop.Name != "Visits" && prop.Name!= "Images" && prop.Name != "Patient" && prop.Name !="Visit")
                        if (prop.PropertyType == typeof(DateTime))
                        {
                            DateTime dt =  Convert.ToDateTime(prop.GetValue(item));
                            row[prop.Name] = dt.ToString("dd-MM-yyyy");
                        }
                        else if(prop.PropertyType ==typeof(users))
                        {
                            //users us = (users)prop.GetValue(item);
                            //row[prop.Name] = us.user_id;
                        }
                        else if(prop.PropertyType == typeof(Person))
                        {

                        }
                        else
                            if (prop.PropertyType == typeof(Patient))
                            {
                            }
                            else if(prop.PropertyType == typeof(visit))
                            {
                            }
                            else if (prop.PropertyType == typeof(obs))
                            {
                            }
                            else if (prop.PropertyType == typeof(ReportType))
                            { }
                            else if (prop.PropertyType == typeof(ISet<PatientDiagnosis>))
                            { }
                            else if (prop.PropertyType == typeof(ISet<patient_identifier>))
                            {
                                ISet<patient_identifier> dt = (prop.GetValue(item)) as ISet<patient_identifier>;
                                row[prop.Name] = dt.First().value;
                            }
                            else if (prop.PropertyType == typeof(ISet<person_attribute>))
                            { }
                            else if (prop.PropertyType == typeof(ISet<PersonAddressModel>))
                            { }
                            else
                                if (prop.PropertyType == typeof(eye_fundus_image))
                                { }
                                else
                                {
                                    //string a = prop.GetValue(item).ToString();

                                    row[prop.Name] = (prop.GetValue(item) ?? DBNull.Value);
                                }
                    
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static DataRow ToDataRow<T>(this T data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            DataRow row = table.NewRow();

            foreach (PropertyDescriptor prop in properties)
            {
                row[prop.Name] = prop.GetValue(data) ?? DBNull.Value;
            }

            table.Rows.Add(row);

            return table.Rows[0];
        }
    }
}
