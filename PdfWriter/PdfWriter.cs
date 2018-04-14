using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PdfWriter
{
    public class PdfWriter
    {
        public PdfWriter()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var path = "PdfWriter.Forms.1040.1040Rev2017.pdf";
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (PdfDocument pdfDoc = PdfReader.Open(stream))
            {
                var pdfFields = pdfDoc.AcroForm.Fields;
                //To allow appearance of the fields
                if (pdfDoc.AcroForm.Elements.ContainsKey("/NeedAppearances") == false)
                {
                    pdfDoc.AcroForm.Elements.Add(
                        "/NeedAppearances",
                        new PdfSharp.Pdf.PdfBoolean(true));
                }
                else
                {
                    pdfDoc.AcroForm.Elements["/NeedAppearances"] =
                        new PdfSharp.Pdf.PdfBoolean(true);
                }

                //To set the readonly flags for fields to their original values
                bool flag = false;

                //Iterate through the fields from PDF
                for (int i = 0; i < pdfFields.Count(); i++)
                {
                    //Get the current PDF field
                    var pdfField = pdfFields[i] as PdfTextField;

                    if (pdfField == null) continue;

                    flag = pdfField.ReadOnly;

                    //Check if it is readonly and make it false
                    if (pdfField.ReadOnly)
                    {
                        pdfField.ReadOnly = false;
                    }

                    pdfField.Value = new PdfSharp.Pdf.PdfString(pdfField.Name);

                    //Set the Readonly flag back to the field
                    pdfField.ReadOnly = flag;
                }

                //Save the PDF to the output destination
                pdfDoc.Save("D:\\Downloads\\test.pdf");
                pdfDoc.Close();
            }
        }
    }
}
