using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace LabsterHub
{
    /// <summary>
    /// Interaction logic for MolecularTool.xaml
    /// </summary>
    /// 

    public partial class MolecularTool : UserControl
    {

        private List<Atom> atoms;
        private List<Bound> bounds;

        List<string> atomsCreated;

        public MolecularTool()
        {
            InitializeComponent();
            atoms = new List<Atom>();
            bounds = new List<Bound>();
            atomsCreated = new List<string>();
        }

        private void Button_Molecule_Convert_Click(object sender, RoutedEventArgs e)
        {

            var path = @"C:\Users\Labster\Desktop\untitled.cml";

            XDocument doc = XDocument.Load(path);

            var atomsArray = XmlUtils.Elements(doc, "atom");
            foreach (var a in atomsArray)
            {
                atoms.Add(new LabsterHub.Atom(a));
            }

            var boundsArray = XmlUtils.Elements(doc, "bond");
            foreach (var b in boundsArray)
            {
                bounds.Add(new LabsterHub.Bound(b));
            }

            ConverterResultTextBlock.Text = MoleculeBuilder.BuildMoleculeData(doc);

            ReplaceInEngine();

            return;
        }

        private void ReplaceInEngine()
        {
            var path = @"C:\Users\Labster\Labster\Simulations\Simulations\Engine_ALO_Alcohols.xml";
            XDocument document = XDocument.Load(path);

            // find element and replace
            var elements = XmlUtils.Elements(document, "Element");
            foreach (var e in elements)
            {
                if (XmlUtils.GetId(e) == "Molecule_Ethyl_butanoate")
                {
                    var replacement = XElement.Parse(ConverterResultTextBlock.Text);
                    e.ReplaceWith(replacement);
                    break;
                }
            }

            var settings = new XmlWriterSettings
            {
                Encoding = new System.Text.UTF8Encoding(false),
                Indent = true,
                IndentChars = "\t"
            };
            using (var writer = XmlWriter.Create(path, settings))
            {
                document.Save(writer);
            }

        }

        private void DirectoryPathtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var path = DirectoryPathtextBox.Text;
            if (Directory.Exists(path) == true)
            {

            }
        }

        private string GenerateXmlMoleculeStructure()
        {

            atomsCreated.Clear();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("<Element Id=\"Molecule_Ethyl_butanoate\" SourceId=\"SimpleElement\" GameObjectId=\"Molecule\" Tags=\"Molecule\" >");

            sb.AppendLine("<Fields>");
            sb.AppendLine("<Field Id=\"Field_CarbonAtom_Source\"       Type=\"string\" Value=\"AtomBlack\"/>");
            sb.AppendLine("<Field Id=\"Field_HydrogenAtom_Source\"     Type=\"string\" Value=\"AtomBlue\"/>");
            sb.AppendLine("<Field Id=\"Field_OxygenAtom_Source\"       Type=\"string\" Value=\"AtomRed\"/>");
            sb.AppendLine("<Field Id=\"Field_CarbonBond_Source\"       Type=\"string\" Value=\"AtomicBondSingleBlack\"/>");
            sb.AppendLine("<Field Id=\"Field_CarbonDoubleBond_Source\" Type=\"string\" Value=\"AtomicBondDoubleBlack\"/>");
            sb.AppendLine("<Field Id=\"Field_HydrogenBond_Source\"     Type=\"string\" Value=\"AtomicBondSingleBlue\"/>");
            sb.AppendLine("<Field Id=\"Field_OxygenBond_Source\"       Type=\"string\" Value=\"AtomicBondSingleRed\"/>");
            sb.AppendLine("<Field Id=\"Field_OxygenDoubleBond_Source\" Type=\"string\" Value=\"AtomicBondDoubleRed\"/>");
            sb.AppendLine("<Field Id=\"Field_Atom_Radius\"             Value=\"0.075\" />");
            sb.AppendLine("<Field Id=\"Field_Bond_Height\"             Value=\"0.03\" />");
            sb.AppendLine("</Fields>");

            sb.AppendLine("<Elements>");

            foreach (var a in atoms)
            {
                atomsCreated.Add(a.id);
                sb.AppendLine(CreateAtom(a));
                sb.AppendLine("<Elements>");

                foreach (var b in bounds)
                {
                    if (b.boundA == a.id)
                    {
                        sb.AppendLine("<Element Id=\"Connection_" + b.boundA + "_" + b.boundB + "\" SourceId=\"SimpleElement\" Rotation=\"180\">");
                        sb.AppendLine("</Element>");

                        sb.AppendLine("<Element Id=\"Bond_" + b.boundA + "_" + b.boundB + "\" SourceId=\"{Binding Value, ElementId=" + getAtomSource(a) + "}\" Position=\"0,0\" Rotation=\"0\" SurfaceOffset=\"0\">");

                        // Calculate the rotation and position of this element

                        // sb.AppendLine(CreateAtomFromBound(b));

                        sb.AppendLine("</Element>");
                    }
                }

                sb.AppendLine("</Elements>");
                sb.AppendLine("</Element>");
            }

            sb.AppendLine("</Elements>");

            sb.AppendLine("</Element>");

            return sb.ToString();
        }

        string getAtomSource(Atom a)
        {
            switch (a.type)
            {
                case "C": return "Field_CarbonAtom_Source";
                case "H": return "Field_HydrogenAtom_Source";
                case "O": return "Field_OxygenAtom_Source";
            }
            throw new System.Exception("Atom with type [" + a.type + "] not supported");
        }

        string getAtomSourceFromId(string id)
        {
            Atom a = null;

            foreach (var aa in atoms)
            {
                if (aa.id == id)
                {
                    a = aa;
                    break;
                }
            }

            switch (a.type)
            {
                case "C": return "Field_CarbonAtom_Source";
                case "H": return "Field_HydrogenAtom_Source";
                case "O": return "Field_OxygenAtom_Source";
            }
            throw new System.Exception("Atom with type [" + a.type + "] not supported");
        }

        string getBondSource(Atom a)
        {
            switch (a.type)
            {
                case "C": return "Field_CarbonAtom_Source";
                case "H": return "Field_HydrogenAtom_Source";
                case "O": return "Field_OxygenAtom_Source";
            }
            throw new System.Exception("Atom with type [" + a.type + "] not supported");
        }

        private string CreateAtomFromBound(Bound b)
        {
            Atom a = null;

            foreach (var aa in atoms)
            {
                if (aa.id == b.boundB)
                {
                    a = aa;
                    break;
                }
            }

            if (a == null)
            {
                throw new System.Exception("Exception not found");
            }

            return "<Element Id=\"Atom_" + b.boundA + "_" + b.boundB + "\" SourceId=\"{Binding Value, ElementId=" + getAtomSourceFromId(b.boundB) + "}\" Position=\"" + a.x + ", " + a.z + "\" Rotation=\"0\" SurfaceOffset=\"0\" >";
        }

        private string CreateAtom(Atom a)
        {
            return "<Element Id=\"Atom_" + a.id + "\" SourceId=\"{Binding Value, ElementId=" + getAtomSource(a) + "}\" Position=\"" + a.x + ", " + a.z + "\" Rotation=\"0\" SurfaceOffset=\"0\" >";
        }

    }
}
