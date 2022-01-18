using System;
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
    }
}
