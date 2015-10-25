using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Tools;
using Adam.Core.Search;
using Adam.Core.Management;
using Adam.Core.Classifications;
using Adam.Core.Records;

namespace Task2
{
    public class Finder
    {
        private Application application;

        public Finder()
        {
            application = new Application();

            var result = application.LogOn("TRAINING", "Administrator", "P2ssw0rd");
            if (result != LogOnStatus.LoggedOn)
                throw new Exception("Can't log on");
        }

        public List<string> FindUserStartsWith(string begin)
        {
            List<string> result = new List<string>();
            SearchExpression searchExpr = new SearchExpression("Name=" + begin + "*");
            UserCollection userCollection = new UserCollection(application);
            userCollection.Load(searchExpr);
            
            foreach (User user in userCollection)
            {
                result.Add(user.Name);
            }
            return result;
        }

        public List<string> FindUserGroup(string group)
        {
            List<string> result = new List<string>();
            SearchExpression searchExpr = new SearchExpression("Group.Name="+group);
            UserCollection userCollection = new UserCollection(application);
            userCollection.Load(searchExpr);

            foreach (User user in userCollection)
            {
                result.Add(user.Name);
            }
            return result;
        }

        public List<string> FindUserLoggedIn(DateTime date)
        {
            var result = new List<string>();
            SearchExpression searchExpr = new SearchExpression("LastSuccessfulLogOn > " + date.Month + '/' + date.Day + '/'+ date.Year);
            UserCollection userCollection = new UserCollection(application);
            userCollection.Load(searchExpr);

            foreach (User user in userCollection)
            {
                result.Add(user.Name);
            }
            return result;
        }

        public string FindClassificationByName(string name)
        {
            string result;
            var searchExpr = new SearchExpression("Name=" + name);
            var classificationCollection = new ClassificationCollection(application);
            classificationCollection.Load(searchExpr);
            if (classificationCollection.Count > 0)
                result = classificationCollection[0].Name;
            else
                result = "Not found";
            return result;
        }

        public string FindClassificationByPath(string path)
        {
            string result;
            var searchExpr = new SearchExpression("NamePath=" + path);
            var classificationCollection = new ClassificationCollection(application);
            classificationCollection.Load(searchExpr);
            if (classificationCollection.Count > 0)
                result = classificationCollection[0].Name;
            else
                result = "Not found";
            return result;
        }

        public List<string> FindClassificationByPathAndDate(string path, DateTime date)
        {
            //TO DO here subclassification, not root
            var result = new List<string>();
            string id;
            var searchExpr = new SearchExpression("NamePath=" + path);
            var classificationCollection = new ClassificationCollection(application);
            classificationCollection.Load(searchExpr);
            if (classificationCollection.Count > 0)
                id = classificationCollection[0].Id.ToString();
            else
                throw new Exception("Not found");

            searchExpr = new SearchExpression("Root=" + id + " AND CreatedOn > " + date.Month + '/' + date.Day + '/' + date.Year);
             
            classificationCollection = new ClassificationCollection(application);
            classificationCollection.Load(searchExpr);

            foreach (Classification classification in classificationCollection)
            {
                result.Add(classification.Name);
            }
            return result;
        }

        public List<string> FindRecordFromClassifications(string classification)
        {
            var result = new List<string>();
            var searchExpression = new SearchExpression("Classification.NamePath ="+classification);
            var recordCollection = new RecordCollection(application);
            recordCollection.Load(searchExpression);

            foreach (Record record in recordCollection)
            {
                result.Add(record.Id.ToString());
            }
            return result;
        }

        public List<string> FindRecordWithExtension(string extension)
        {
            var result = new List<string>();
            var searchExpression = new SearchExpression("File.Version.Extension = " + extension);
            var recordCollection = new RecordCollection(application);
            recordCollection.Load(searchExpression);

            foreach (Record record in recordCollection)
            {
                result.Add(record.Id.ToString());
            }
            return result;
        }

        public List<string> FindRecordWithFileSize(string size)
        {
            var result = new List<string>();
            var searchExpression = new SearchExpression("File.Version.Filesize > " + size);
            var recordCollection = new RecordCollection(application);
            recordCollection.Load(searchExpression);

            foreach (Record record in recordCollection)
            {
                result.Add(record.Id.ToString());
            }
            return result;
        }

        public List<string> FindRecordWithSpecialTypes(string[] types)
        {
            var result = new List<string>();
            var searchExpr = new SearchExpression("Extension = eps OR Extension = pdf");
            var recordCollection = new RecordCollection(application);
            recordCollection.Load(searchExpr);

            foreach (Record record in recordCollection)
            {
                result.Add(record.Id.ToString());
            }
            return result;
        }

        public List<string> FindRecordWithFieldCreator()
        {
            var result = new List<string>();
            var searchExpression = new SearchExpression("FieldName(\"Creator\") <> \"\"");
            var recordCollection = new RecordCollection(application);
            recordCollection.Load(searchExpression);

            foreach (Record record in recordCollection)
            {
                result.Add(record.Id.ToString());
            }
            return result;
        }
    }
}
