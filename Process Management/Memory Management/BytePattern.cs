using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace Narrative {
  public class BytePattern {
    public String String { get; private set; }
    public Byte?[] Bytes { get; private set; }
    public Int32 Length { get { return Bytes.Length; } }

    public BytePattern ( String ByteString ) {
      String = ByteString;

      List<Byte?> ByteList = new List<Byte?>();
      foreach ( String part in ByteString.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries) ) {
        for ( int i = 0 ; i < part.Length ; i += 2 ) {
          String byteString = part.Substring(i, 2);
            ByteList.Add(
              Byte.TryParse(byteString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out Byte parsedByte)
              ? (Byte?) parsedByte
              : null
            );
        }
      }
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

    public Boolean Match ( Byte[] buffer ) {
      if ( buffer.Length != Length )
        return false;
      for ( int i = 0; i < Length; i++ ) {
        if ( Bytes[i] == null )
          continue;
        if ( buffer[i] != Bytes[i] )
          return false;
      }
      return true;
    }
  }
}
