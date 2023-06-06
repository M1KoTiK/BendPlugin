using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TFlex;
using TFlex.Model;

namespace BendPlugin
{


    public class Bend : PluginFactory
    {
        public override string Name => "Разгибание борта";

        public override Guid ID => new Guid("{62F0C0D7-F516-4b69-837F-F146265981EA}");

        public override Plugin CreateInstance()
        {
            return new BendPlugin(this);
        }
    }
    enum PluginCommand
    {
        TestCommand = 1
    }

    public class BendPlugin : Plugin
    {
        private TFlex.Model.ModelObject selectedObject; 
        System.Drawing.Icon LoadIconResource(string name)
        {
            System.IO.Stream stream = GetType().Assembly.
                GetManifestResourceStream("BendPlugin.Resources." + name + ".ico");
            return new System.Drawing.Icon(stream);
        }

        private Logs logs;
        public BendPlugin(PluginFactory Factory) : base(Factory)
        {
            selectedObject = null;
            logs = new Logs("C:\\Users\\User\\Desktop\\123\\");
            logs.ClearFile();

        }
        protected override void OnCreateTools()
        {
            RegisterCommand((int)PluginCommand.TestCommand, "Тестовая Команда", LoadIconResource("image"), LoadIconResource("image"));

            //Добавляем кнопки во вкладку "Приложения" в ленте
            TFlex.RibbonGroup ribbonGroup = TFlex.RibbonBar.ApplicationsTab.AddGroup("Разгибание борта");
            ribbonGroup.AddButton((int)PluginCommand.TestCommand, this);

            //просто подключаем плагин к текущему документу
            if (TFlex.Application.ActiveDocument != null)
                TFlex.Application.ActiveDocument.AttachPlugin(this);

        }
        
        protected override void NewDocumentCreatedEventHandler(DocumentEventArgs args)
        {
            //AttachPlugin нужно вызвать обязательно, иначе от данного документа не будут приходить уведомления о событиях
            args.Document.AttachPlugin(this);
            //logs.WriteAndSave($"Создание нового документа1 -> {args.Document.Title}\n");

        }

        protected override void ObjectSelectionChangedEventHandler(ObjectEventArgs args)
        {
            var displayName = args.Object.DisplayName;
            var type = args.Object.GroupType;
            //logs.WriteAndSave($"Выбор объекта -> {displayName} (object - {args.Object.GroupType})\n");
            selectedObject = args.Object;
        }

        protected override void OnCommand(Document document, int id)
        {
            switch ((PluginCommand)id)
            {
                default:
                    base.OnCommand(document, id);
                    break;
                case PluginCommand.TestCommand:
                    logs.WriteAndSave("Нажата тестовая команда\n");
                    if(selectedObject.GroupType == ObjectType.Topol)
                    {
                        //logs.WriteAndSave("Выбран топлогический элемент^\n");                        
                        var topol = selectedObject as TFlex.Model.Model3D.TopolReference;
                        if (topol.GeometryType == TFlex.Model.Model3D.Geometry.GeometryType.Edge)
                        {
                            logs.WriteAndSave("\tВыбрано ребро\n");
                        }

                        if(topol.GeometryType == TFlex.Model.Model3D.Geometry.GeometryType.Face)
                        {
                            logs.WriteAndSave("\tВыбрана грань\n");
                            document.BeginChanges("test");
                            //
                            document.EndChanges();
                        }
                    }
                    break;

            }
        }

    }
}
    