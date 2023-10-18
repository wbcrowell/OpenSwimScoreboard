using System.Collections.Generic;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using OpenSwimScoreboard.Forms;
using System.Threading;
using Microsoft.Extensions.Hosting;

namespace OpenSwimScoreboard
{
    public class OSSApplicationContext : ApplicationContext
    {
        private void onFormClosed(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count == 0)
            {
                ExitThread();
            }
        }

        private OSSApplicationContext() { }

        public OSSApplicationContext(IHost hubHost)
        {
            var forms = new List<Form>() 
            {
                new MainForm(),
                new MonitorForm(hubHost),
            };
            foreach (var form in forms)
            {
                form.FormClosed += onFormClosed;
            }

            forms[0].Show();
        }
    }
}
