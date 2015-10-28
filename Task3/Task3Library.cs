using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Tools;
using Adam.Core.Search;
using Adam.Core.Classifications;
using Adam.Core.Fields;
using Adam.Core.Records;
using Adam.Core.Settings;
using Adam.Core.Management;

namespace Task3
{
    public class Task3Library
    {
        Application app;

        public Task3Library()
        {
            app = new Application();

            var result = app.LogOn("TRAINING", "Administrator", "P2ssw0rd");
            if (result != LogOnStatus.LoggedOn)
                throw new Exception("Can't log on");
        }

        public void AddFieldToClassification()
        {
            var clHelper = new ClassificationHelper(app);
            var classificationId = clHelper.GetId(new SearchExpression("AlexanderChechet"));
            Classification cl = new Classification(app);
            if (classificationId == null)
                throw new Exception("Can't find classification");
            cl.Load(classificationId.Value);

            var fieldHelper = new FieldDefinitionHelper(app);
            var fieldId = fieldHelper.GetId(new SearchExpression("Name=AlexanderChechet_multi"));

            if (fieldId == null)
                throw new Exception("Can't find field");
            cl.RegisteredFields.Add(fieldId.Value);
            cl.Save();
        }

        public void AddFilesToClassification()
        {
            var clHelper = new ClassificationHelper(app);
            var clId = clHelper.GetId(new SearchExpression("AlexanderChechet"));
            Classification cl = new Classification(app);
            if (clId == null)
                throw new Exception("Can't find classification");
            var descriptions = new Dictionary<string, string>() 
            {
                {"English", "123"},
                {"Russian", "456"},
                {"German", "789"},
            };
            AddRecord(@"D:\Films\Pictures\2Ax0212.jpg", clId.Value, descriptions);
            AddRecord(@"D:\Films\Pictures\1079.jpg",clId.Value, descriptions);
            AddRecord(@"D:\Films\Pictures\2006-000.jpg",clId.Value, descriptions);
            AddRecord(@"D:\Films\Pictures\entry.jpg",clId.Value, descriptions);
            
        }

        private void AddRecord(string path, Guid clId, Dictionary<string, string> descriptions)
        {
            var record = new Record(app);

            record.AddNew();
            record.Files.AddFile(path);
            record.Classifications.Add(clId);

            Language language = new Language(app);
            foreach(var item in descriptions)
            {
                language.Load(item.Key);
                Guid id = language.Id;
                record.Fields.GetField<TextField>("AlexanderChechet_multi", id).SetValue(item.Value);
            }
            
            record.Save();
        }

        public void CreateTextSettingDefinition()
        {
            var setting = new TextSettingDefinition(app);
            setting.AddNew();

            setting.DefaultValue = "Default value tipo";

            setting.AllowSystemSetting = true;
            setting.AllowUserSetting = true;

            setting.Name = "AlexanderChechetSetting";

            setting.Save();
        }

        public void EditSystemValue(string newValue)
        {
            var setting = new TextSetting(app);

            setting.LoadSystemValue("AlexanderChechetSetting");
            setting.Value = newValue;
            setting.Save();
        }

        public void EditUserValue(string newValue, string username)
        {
            var setting = new TextSetting(app);

            var userHelper = new UserHelper(app);
            var userId = userHelper.GetId(new SearchExpression(username));
            if (userId.HasValue)
            {
                setting.LoadUserValue("AlexanderChechetSetting", userId.Value);
                setting.Value = newValue;
                setting.Save();
            }
        }

        public string GetSystemValue()
        {
            var setting = (TextSetting)app.GetSetting("AlexanderChechetSetting");
            return setting.Value;
        }

        public string GetUserValue(string username)
        {
            string result = null;
            var userHelper = new UserHelper(app);
            var userId = userHelper.GetId(new SearchExpression(username));
            if (userId.HasValue)
            {
                using (var context = new ImpersonateContext(app, userId.Value))
                {
                    var setting = (TextSetting)app.GetSetting("AlexanderChechetSetting");
                    result = setting.Value;
                }
            }
            return result;
        }
    }
}
