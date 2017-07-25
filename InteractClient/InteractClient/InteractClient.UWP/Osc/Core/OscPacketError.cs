
namespace InteractClient.UWP.Osc
{
  #region Osc Packet Error

  public enum OscPacketError
  {
    None,

    InvalidSegmentLength,

    MissingAddress,
    MissingComma,
    MissingTypeTag,
    MalformedTypeTag,

    ErrorParsingArgument,
    ErrorParsingBlob,
    ErrorParsingString,
    ErrorParsingSymbol,
    ErrorParsingInt32,
    ErrorParsingInt64,
    ErrorParsingSingle,
    ErrorParsingDouble,

    ErrorParsingColor,
    ErrorParsingChar,
    ErrorParsingMidiMessage,
    ErrorParsingOscTimeTag,

    UnknownArguemntType,

    MissingBundleIdent,
    InvalidBundleIdent,
    InvalidBundleMessageHeader,
    ErrorParsingPacket,
    InvalidBundleMessageLength,
  }

  #endregion
}
