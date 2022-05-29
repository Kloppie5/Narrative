using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace Narrative {
    public class BytePattern {
        public String String { get; private set; }
        public Byte?[] Bytes { get; private set; }

        public BytePattern ( String ByteString ) {
            String = ByteString;

            List<Byte?> ByteList = new List<Byte?>();
            var singleByteStrings = ByteString.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach ( var singleByteString in singleByteStrings )
                ByteList.Add(
                    Byte.TryParse(singleByteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out Byte parsedByte)
                    ? (Byte?) parsedByte
                    : null
                );

            Bytes = ByteList.ToArray();
        }

        public String PatternString ( ) {
            StringBuilder ByteString = new StringBuilder();
            for ( int i = 0; i < Bytes.Length; i++ ) {
                if ( i > 0 )
                    ByteString.Append(" ");
                if ( Bytes[i] == null )
                    ByteString.Append("??");
                else
                    ByteString.Append($"{Bytes[i]:X2}");
            }
            return ByteString.ToString();
        }
    }
}
