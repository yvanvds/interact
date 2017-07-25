using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  /// <summary>
	/// Type of address part
	/// </summary>
	public enum OscAddressPartType
  {
    /// <summary>
    /// Address seperator char i.e. '/'  
    /// </summary>
    AddressSeparator,

    /// <summary>
    /// Address wildcared i.e. '//'
    /// </summary>
    AddressWildcard,

    /// <summary>
    /// Any string literal i.e [^\s#\*,/\?\[\]\{}]+ 
    /// </summary>
    Literal,

    /// <summary>
    /// Either single char or anylength wildcard i.e '?' or '*'
    /// </summary>
    Wildcard,

    /// <summary>
    /// Char span e.g. [a-z]+
    /// </summary>
    CharSpan,

    /// <summary>
    /// List of literal matches
    /// </summary>
    List,

    /// <summary>
    /// List of posible char matches e.g. [abcdefg]+
    /// </summary>
    CharList,
  }
}
