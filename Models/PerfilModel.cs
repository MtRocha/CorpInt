namespace Intranet_NEW.Models
{
    public class PerfilModel
    {
        public static readonly int[] Planejamento = { 1016, 1051, 1067, 1076, 1210, 1233, 20, 21 };
        public static readonly int[] Operacional = {  1001, 1002, 1006, 1010, 1011, 1012,1034, 1035, 1039, 1043, 1049, 1079,1223, 1228};
        public static readonly int[] Qualidade = { 1052, 1054, 1152, 1219, 1229 };

        private static readonly Dictionary<int, int[]> PerfisDicionario = new Dictionary<int, int[]>
        {
            { 1040, new int[] { 1016, 1051, 1067, 1076, 1210, 1233, 20, 21 } }, // Planejamento
            { 1011, new int[] { 1001, 1002, 1006, 1010, 1011, 1012, 1034, 1035, 1039, 1043, 1049, 1079, 1223, 1228 } }, // Operacional
            { 1054, new int[] { 1052, 1054, 1152, 1219, 1229 } } // Qualidade
        };

        public static int ObterChavePorValor(int valor)
        {
            return PerfisDicionario
                .FirstOrDefault(kvp => kvp.Value.Contains(valor))
                .Key;
        }


    }
}
