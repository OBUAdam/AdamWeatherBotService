namespace AdamWeatherBotService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AdamWeatherBotServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AdamWeatherBotServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AdamWeatherBotServiceProcessInstaller
            // 
            this.AdamWeatherBotServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.AdamWeatherBotServiceProcessInstaller.Password = null;
            this.AdamWeatherBotServiceProcessInstaller.Username = null;
            // 
            // AdamWeatherBotServiceInstaller
            // 
            this.AdamWeatherBotServiceInstaller.Description = "The AdamWeatherBot Service";
            this.AdamWeatherBotServiceInstaller.DisplayName = "AdamWeatherBotService";
            this.AdamWeatherBotServiceInstaller.ServiceName = "AdamWeatherBotService";
            this.AdamWeatherBotServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AdamWeatherBotServiceProcessInstaller,
            this.AdamWeatherBotServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AdamWeatherBotServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AdamWeatherBotServiceInstaller;
    }
}