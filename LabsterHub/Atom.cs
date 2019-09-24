using System.Xml.Linq;

namespace LabsterHub
{
    public class Atom
    {

        public string id;
        public string type;
        public float x;
        public float y;
        public float z;

        public Atom(XElement e)
        {
            id = XmlUtils.GetAttribute(e, "id").Value;
            type = XmlUtils.GetAttribute(e, "elementType").Value;
            x = float.Parse(XmlUtils.GetAttribute(e, "x3").Value);
            y = float.Parse(XmlUtils.GetAttribute(e, "y3").Value);
            z = float.Parse(XmlUtils.GetAttribute(e, "z3").Value);
        }

    }

    public class Bound
    {

        public string boundA;
        public string boundB;

        public Bound(XElement e)
        {
            var atomRefs = XmlUtils.GetAttribute(e, "atomRefs2").Value;
            string[] words = atomRefs.Split(' ');
            boundA = words[0];
            boundB = words[1];
        }

    }

    public static class AtomBuilder
    {

        //Variables
        private static float Field_Cos_0 = 1.0f;
        private static float Field_Sin_0 = 0.0f;
        private static float Field_Cos_90 = 0.01f;
        private static float Field_Sin_90 = 1.0f;

        private static float Field_Cos_30 = 0.866f;
        private static float Field_Sin_30 = 0.5f;
        private static float Field_Cos__30 = -0.866f;
        private static float Field_Sin__30 = -0.5f;

        private static float Field_Cos__35_25 = -0.816f;
        private static float Field_Sin__35_25 = -0.577f;
        private static float Field_Cos_35_25 = 0.816f;
        private static float Field_Sin_35_25 = 0.577f;

        private static float Field_Cos_40_5 = 0.7604f;
        private static float Field_Sin_40_5 = 0.649f;


        // Constant values defining for calculations
        private static float Field_Atom_Radius = 0.075f;
        private static float Field_Bond_Height = 0.03f;

        // determine Variables used accross calculations
        private static float Field_SetCurrent_CosA = 0.01f;
        private static float Field_SetCurrent_SinA = 0.01f;
        private static int Field_CurrentCoreX_Atom = 0;

        public static float Field_2R_Plus_H()
        {
            var arg1 = 2f * Field_Atom_Radius;
            var arg2 = 1f * Field_Bond_Height;
            var result = arg1 + arg2;
            return result;
        }

        // First Bond.
        public static float Field_FirstBond_X()
        {
            var arg1 = Field_Atom_Radius;
            var arg2 = Field_SetCurrent_CosA;
            var result = arg1 * arg2;
            return result;
        }
        public static float Field_FirstBond_Y()
        {
            var arg1 = Field_Atom_Radius;
            var arg2 = Field_SetCurrent_SinA;
            var result = arg1 * arg2;
            return result;
        }

        // Target Atom
        public static float Field_NextAtom_X()
        {
            var arg1 = Field_2R_Plus_H();
            var arg2 = Field_SetCurrent_CosA;
            var result = arg1 * arg2;
            return result;
        }
        public static float Field_NextAtom_Y()
        {
            var arg1 = Field_2R_Plus_H();
            var arg2 = Field_SetCurrent_SinA;
            var result = arg1 * arg2;
            return result;
        }

        // Core Atom
        public static float Field_CoreAtom_X()
        {
            var arg1 = Field_NextAtom_X();
            var arg2 = Field_CurrentCoreX_Atom;
            var result = arg1 + arg2;
            return result;
        }
        public static float Field_CoreAtom_Y()
        {
            var arg1 = Field_2R_Plus_H();
            var arg2 = Field_SetCurrent_SinA;
            var result = arg1 + arg2;
            return result;
        }

        // Second Bond
        public static float Field_SecondBond_X()
        {
            var arg11 = Field_Atom_Radius;
            var arg12 = Field_SetCurrent_CosA;
            var arg1 = arg11 * arg12;
            var arg2 = -1.0f;
            var result = arg1 * arg2;
            return result;
        }
        public static float Field_SecondBond_Y()
        {
            var arg11 = Field_Atom_Radius * Field_SetCurrent_SinA;
            var arg12 = 0.5f * Field_Bond_Height;
            var arg1 = arg11 + arg12;
            var arg2 = -1.0f;
            var result = arg1 * arg2;
            return result;
        }

        // TODO

        // Positions of two atoms and two bonds

        // Positional State for Chain Atom

        // State for bond rotation

        // Connection Core1 - Side1

    }


}