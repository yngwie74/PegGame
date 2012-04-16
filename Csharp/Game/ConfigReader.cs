namespace Game
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ConfigReader
    {
        #region Constants and Fields

        private const int MaxMissing = 100;

        private readonly TextReader reader;

        #endregion

        #region Constructors and Destructors

        public ConfigReader(TextReader reader)
        {
            this.reader = reader;
        }

        #endregion

        #region Public Methods and Operators

        public Config ReadConfig()
        {
            int height = 0, width = 0, goal = 0;
            var missing = new List<Coord>();

            try
            {
                int nmissing;
                ParseTwo(this.GetNextLine(), "La altura", out height, "El ancho", out width);
                ParseOne(this.GetNextLine(), "La columna meta", out goal);
                ParseOne(this.GetNextLine(), "El número de pijas faltantes", out nmissing);
                nmissing = AdjustToMax(nmissing);
                this.ReadCoords(nmissing, missing);
            }
            catch (Exception e)
            {
            }

            return new Config(height, width, goal, missing);
        }

        #endregion

        #region Methods

        private static int AdjustToMax(int nmissing)
        {
            if (nmissing < 0)
            {
                nmissing = 0;
            }
            if (nmissing > MaxMissing)
            {
                nmissing = MaxMissing;
            }
            return nmissing;
        }

        private static int ParseInt(string desc, string val)
        {
            int result;
            if (!int.TryParse(val.Trim(), out result))
            {
                var message = string.Format(
                    "Valor inválido en el archivo de configuración {0}", val);
                throw new ArgumentOutOfRangeException(message, desc);
            }
            return result;
        }

        private static void ParseOne(string line, string desc, out int value)
        {
            if (!string.IsNullOrEmpty(line))
            {
                value = ParseInt(desc, line);
            }
            else
            {
                var message = string.Format(
                    "No se encontró el valor de {0} en el archivo de configuración", desc);
                throw new ArgumentNullException(message);
            }
        }

        private static void ParseTwo(
            string line, string desc0, out int val0, string desc1, out int val1)
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            ParseOne(parts[0], desc0, out val0);
            ParseOne(parts.Length >= 2 ? parts[1] : string.Empty, desc1, out val1);
        }

        private string GetNextLine()
        {
            return this.reader.ReadLine() ?? string.Empty;
        }

        private Coord ReadCoord(int index)
        {
            int r, c;
            ParseTwo(
                this.GetNextLine(),
                string.Format("r{0}", index), out r,
                string.Format("c{0}", index), out c);
            return new Coord(r, c);
        }

        private void ReadCoords(int count, ICollection<Coord> list)
        {
            for (var i = 0; i < count; i++)
            {
                list.Add(this.ReadCoord(i));
            }
        }

        #endregion
    }
}