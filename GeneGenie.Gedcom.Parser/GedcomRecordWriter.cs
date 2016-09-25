// <copyright file="GedcomRecordWriter.cs" company="GeneGenie.com">
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see http:www.gnu.org/licenses/ .
//
// </copyright>
// <author> Copyright (C) 2016 Ryan O'Neill r@genegenie.com </author>
// <author> Copyright (C) 2007-2008 David A Knight david@ritter.demon.co.uk </author>

namespace GeneGenie.Gedcom.Parser
{
    using System;
    using System.Collections;
    using System.Text;

    /// <summary>
    /// Used to save a GedcomDatabase to a GEDCOM file.
    /// </summary>
    public class GedcomRecordWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GedcomRecordWriter"/> class.
        /// Create a GEDCOM writer for saving a database to a GEDCOM file.
        /// </summary>
        public GedcomRecordWriter()
        {
        }

        /// <summary>
        /// Gets or sets the name of the GEDCOM file being written.
        /// </summary>
        public string GedcomFile { get; set; }

        /// <summary>
        /// Gets or sets the database for the file being written.
        /// </summary>
        public GedcomDatabase Database { get; set; }

        /// <summary>
        /// Gets or sets the name of the application that created the GEDCOM file.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the application version that created the GEDCOME file.
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets the application system identifier.
        /// </summary>
        public string ApplicationSystemId { get; set; }

        /// <summary>
        /// Gets or sets the owner name for the software that created the GEDCOM.
        /// </summary>
        public string Corporation { get; set; }

        /// <summary>
        /// Gets or sets the corporation address.
        /// </summary>
        /// <summary>
        /// The corporation address.
        /// </summary>
        public GedcomAddress CorporationAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the information separator when saving.
        /// </summary>
        /// <summary>
        /// <c>true</c> if [allow information separator on save]; otherwise, <c>false</c>.
        /// </summary>
        public bool AllowInformationSeparatorOnSave { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow line tabs save].
        /// </summary>
        /// <summary>
        ///   <c>true</c> if [allow line tabs save]; otherwise, <c>false</c>.
        /// </summary>
        public bool AllowLineTabsSave { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow tabs save].
        /// </summary>
        /// <summary>
        ///   <c>true</c> if [allow tabs save]; otherwise, <c>false</c>.
        /// </summary>
        public bool AllowTabsSave { get; set; }

        /// <summary>
        /// Helper method to output a standard GEDCOM file without needing to create a write.
        /// </summary>
        /// <param name="database">The database to output.</param>
        /// <param name="file">The file path to output to.</param>
        public static void OutputGedcom(GedcomDatabase database, string file)
        {
            var writer = new GedcomRecordWriter();
            writer.WriteGedcom(database, file);
        }

        /// <summary>
        /// Outputs the currently set GedcomDatabase to the currently set file
        /// </summary>
        public void WriteGedcom()
        {
            WriteGedcom(Database, GedcomFile);
        }

        /// <summary>
        /// Outputs a GedcomDatabase to the given file
        /// </summary>
        /// <param name="database">The GedcomDatabase to write</param>
        /// <param name="file">The filename to write to</param>
        public void WriteGedcom(GedcomDatabase database, string file)
        {
            Encoding enc = new UTF8Encoding();
            using (var w = new GedcomStreamWriter(file, false, enc))
            {
                w.AllowInformationSeparatorOnSave = AllowInformationSeparatorOnSave;
                w.AllowLineTabsSave = AllowLineTabsSave;
                w.AllowTabsSave = AllowTabsSave;

                database.Header.Output(w);

                // write records
                foreach (DictionaryEntry entry in database)
                {
                    GedcomRecord record = entry.Value as GedcomRecord;

                    record.Output(w);
                    w.Write(Environment.NewLine);
                }

                w.Write(Environment.NewLine);
                w.WriteLine("0 TRLR");
                w.Write(Environment.NewLine);
            }
        }
    }
}
