using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LabsterHub
{
    class MoleculeBuilder
    {

        class Molecule
        {
            public Atom main = null;
            public List<Atom> a = new List<Atom>();
            public List<Bound> b = new List<Bound>();
        }

        private static List<Atom> atoms = new List<Atom>();
        private static List<Bound> bounds = new List<Bound>();
        private static List<Molecule> molecules = new List<Molecule>();

        public static string BuildMoleculeData(XDocument doc)
        {

            atoms.Clear();
            bounds.Clear();
            molecules.Clear();

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

            foreach (var b in bounds)
            {
                var m = FindMoleculeById(b.boundA);
                if (m == null)
                {
                    m = new Molecule();
                    m.main = FindAtomById(b.boundA);
                    molecules.Add(m);
                }
                var tmp = FindAtomById(b.boundB);
                m.a.Add(tmp);
                m.b.Add(b);
            }

            return GenerateXmlMoleculeStructure();
        }

        private static Atom FindAtomById(string id)
        {
            foreach (var a in atoms)
            {
                if (a.id == id)
                {
                    return a;
                }
            }
            return null;
        }

        private static Molecule FindMoleculeById(string id)
        {
            foreach (var a in molecules)
            {
                if (a.main.id == id)
                {
                    return a;
                }
            }
            return null;
        }

        private static string GenerateXmlMoleculeStructure()
        {

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

            foreach (var m in molecules)
            {
                CreateMolecule(sb, m);
            }
            sb.AppendLine("</Elements>");
            // sb.AppendLine("<StateGroups>");
            // sb.AppendLine("<StateGroup Id=\"Inialize_Core_Position\" DataContext=\"{Binding Source={RelativeSource AncestorLevel=1}}\" >");
            // sb.AppendLine("<States>");
            // sb.AppendLine("<State Id=\"State_Initial_Position\" IsDefault=\"true\" >");
            // sb.AppendLine("<Triggers>");
            // sb.AppendLine("<Trigger>");
            // sb.AppendLine("<Actions>");
            // sb.AppendLine("<SetField FieldId=\"Field_SetX_Coordinate\" Value=\"0.001\" />");
            // sb.AppendLine("<SetField FieldId=\"Field_CurrentCoreX_Atom\" Value=\"{Binding Value, ElementId=Field_SetX_Coordinate}\" />");
            // sb.AppendLine("<Setter Target=\"{Binding Position}\" Value=\"{Binding Value, ElementId=Func_ReturnPositionString}\" />");
            // sb.AppendLine("<Setter Target=\"{Binding SurfaceOffset}\" Value=\"0\" />");
            // sb.AppendLine("</Actions>");
            // sb.AppendLine("</Trigger>");
            // sb.AppendLine("</Triggers>");
            // sb.AppendLine("</State>");
            // sb.AppendLine("</States>");
            // sb.AppendLine("</StateGroup>");
            // sb.AppendLine("</StateGroups>");
            sb.AppendLine("</Element>");
            return sb.ToString();
        }

        private static void CreateMolecule(StringBuilder sb, Molecule m)
        {
            sb.AppendLine("<Element Id=\"Atom_" + m.main.id + "\" SourceId=\"{Binding Value, ElementId=Field_CarbonAtom_Source}\" Position=\"0,0\" Rotation=\"0\" SurfaceOffset=\"0\">");

            sb.AppendLine("<Elements>");

            int i = 0;
            foreach (var b in m.b)
            {

                int angle = 180;

                if (i == 1)
                {
                    angle = -60;
                }
                else if (i == 2)
                {
                    angle = 60;
                }

                i++;


                sb.AppendLine("<Element Id=\"Connection_" + b.boundA + "_" + b.boundB + "\" SourceId=\"SimpleElement\" Rotation=\"" + angle + "\">");
                sb.AppendLine("<Elements>");

                {
                sb.AppendLine("<Element Id=\"Bond_" + b.boundA + "_" + b.boundB + "\" SourceId=\"{Binding Value, ElementId=Field_CarbonBond_Source}\" Position=\"0,0\" Rotation=\"0\" SurfaceOffset=\"0\">");
                sb.AppendLine("<StateGroups>");
                sb.AppendLine("<StateGroup Id=\"StateGroup_Bond_ZRotation\" >");
                sb.AppendLine("<States>");
                sb.AppendLine("<State Id=\"State_ZRotation\" AnimationId=\"ZBondAnimations\" AnimationEndPoint=\"0.8625\" IsDefault=\"true\" >");
                sb.AppendLine("</State>");
                sb.AppendLine("</States>");
                sb.AppendLine("</StateGroup>");
                sb.AppendLine("<StateGroup Id=\"Inialize_Position\" DataContext=\"{Binding Source={RelativeSource AncestorLevel=1}}\" >");
                sb.AppendLine("<States>");
                sb.AppendLine("<State Id=\"State_Initial_Position\" IsDefault=\"true\" >");
                sb.AppendLine("<Triggers>");
                sb.AppendLine("<Trigger>");
                sb.AppendLine("<Actions>");
                sb.AppendLine("<SetField FieldId=\"Field_SetCurrent_CosA\" Value=\"{Binding Value, ElementId=Field_Cos_40-5}\" />");
                sb.AppendLine("<SetField FieldId=\"Field_SetCurrent_SinA\" Value=\"{Binding Value, ElementId=Field_Sin_40-5}\" />");
                sb.AppendLine("<SetField FieldId=\"Field_SetX_Coordinate\" Value=\"{Binding Value, ElementId=Field_FirstBond_X}\" />");
                sb.AppendLine("<Setter Target=\"{Binding Position}\" Value=\"{Binding Value, ElementId=Func_ReturnPositionString}\" />");
                sb.AppendLine("<Setter Target=\"{Binding SurfaceOffset}\" Value=\"{Binding Value, ElementId=Field_FirstBond_Y}\" />");
                sb.AppendLine("</Actions>");
                sb.AppendLine("</Trigger>");
                sb.AppendLine("</Triggers>");
                sb.AppendLine("</State>");
                sb.AppendLine("</States>");
                sb.AppendLine("</StateGroup>");
                sb.AppendLine("</StateGroups>");
                sb.AppendLine("</Element>");
                }
            
            
                sb.AppendLine("<Element Id=\"Atom_" + b.boundA + "_" + b.boundB + "\" SourceId=\"{Binding Value, ElementId=Field_HydrogenAtom_Source}\" Position=\"0,0\" Rotation=\"0\" SurfaceOffset=\"0\">");
                sb.AppendLine("<Elements>");
                sb.AppendLine("<Element Id=\"Bond_" + b.boundA + "_" + b.boundB + "\" SourceId=\"{Binding Value, ElementId=Field_HydrogenBond_Source}\" Position=\"0,0\" Rotation=\"0\" SurfaceOffset=\"0\" >");
                    sb.AppendLine("<StateGroups>");
                    sb.AppendLine("<StateGroup Id=\"StateGroup_Bond_ZRotation\">");
                    sb.AppendLine("<States>");
                    sb.AppendLine("<State Id=\"State_ZRotation\" AnimationId=\"ZBondAnimations\" AnimationEndPoint=\"0.8625\" IsDefault=\"true\" ></State>");
                    sb.AppendLine("</States>");
                    sb.AppendLine("</StateGroup>");
                    sb.AppendLine("<StateGroup Id=\"Inialize_Position\" DataContext=\"{Binding Source={RelativeSource AncestorLevel=1}}\" >");
                    sb.AppendLine("<States>");
                    sb.AppendLine("<State Id=\"State_Initial_Position\" IsDefault=\"true\" >");
                    sb.AppendLine("<Triggers>");
                    sb.AppendLine("<Trigger>");
                    sb.AppendLine("<Actions>");
                    sb.AppendLine("<SetField FieldId=\"Field_SetCurrent_CosA\" Value=\"{Binding Value, ElementId=Field_Cos_40-5}\" />");
                    sb.AppendLine("<SetField FieldId=\"Field_SetCurrent_SinA\" Value=\"{Binding Value, ElementId=Field_Sin_40-5}\" />");
                    sb.AppendLine("<SetField FieldId=\"Field_SetX_Coordinate\" Value=\"{Binding Value, ElementId=Field_SecondBond_X}\" />");
                    sb.AppendLine("<Setter Target=\"{Binding Position}\" Value=\"{Binding Value, ElementId=Func_ReturnPositionString}\" />");
                    sb.AppendLine("<Setter Target=\"{Binding SurfaceOffset}\" Value=\"{Binding Value, ElementId=Field_SecondBond_Y}\" />");
                    sb.AppendLine("</Actions>");
                    sb.AppendLine("</Trigger>");
                    sb.AppendLine("</Triggers>");
                    sb.AppendLine("</State>");
                    sb.AppendLine("</States>");
                    sb.AppendLine("</StateGroup>");
                    sb.AppendLine("</StateGroups>");
                    sb.AppendLine("</Element>");
                    sb.AppendLine("</Elements>");
                    sb.AppendLine("<StateGroups>");
                    sb.AppendLine("<StateGroup Id=\"Inialize_Position\" DataContext=\"{Binding Source={RelativeSource AncestorLevel=1}}\">");
                    sb.AppendLine("<States>");
                    sb.AppendLine("<State Id=\"State_Initial_Position\" IsDefault=\"true\">");
                    sb.AppendLine("<Triggers>");
                    sb.AppendLine("<Trigger>");
                    sb.AppendLine("<Actions>");
                    sb.AppendLine("<SetField FieldId=\"Field_SetCurrent_CosA\" Value=\"{Binding Value, ElementId=Field_Cos_40-5}\" />");
                    sb.AppendLine("<SetField FieldId=\"Field_SetCurrent_SinA\" Value=\"{Binding Value, ElementId=Field_Sin_40-5}\" />");
                    sb.AppendLine("<SetField FieldId=\"Field_SetX_Coordinate\" Value=\"{Binding Value, ElementId=Field_NextAtom_X}\" />");
                    sb.AppendLine("<Setter Target=\"{Binding Position}\" Value=\"{Binding Value, ElementId=Func_ReturnPositionString}\" />");
                    sb.AppendLine("<Setter Target=\"{Binding SurfaceOffset}\" Value=\"{Binding Value, ElementId=Field_NextAtom_Y}\" />");
                    sb.AppendLine("</Actions>");
                    sb.AppendLine("</Trigger>");
                    sb.AppendLine("</Triggers>");
                    sb.AppendLine("</State>");
                    sb.AppendLine("</States>");
                    sb.AppendLine("</StateGroup>");
                    sb.AppendLine("</StateGroups>");
                sb.AppendLine("</Element>");
                sb.AppendLine("</Elements>");
                sb.AppendLine("</Element>");
            }
            sb.AppendLine("</Elements>");
            sb.AppendLine("</Element>");
        }

    }
}
