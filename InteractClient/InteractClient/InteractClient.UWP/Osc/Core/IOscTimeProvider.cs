﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractClient.UWP.Osc
{
  /// <summary>
	/// Provides osc timeing information
	/// </summary>
	public interface IOscTimeProvider
  {
    /// <summary>
    /// Get the current time 
    /// </summary>
    OscTimeTag Now { get; }

    /// <summary>
    /// Is the supplied time within the current frame according to this time provider
    /// </summary>
    /// <param name="time">the time to check</param>
    /// <returns>true if within the frame else false</returns>
    bool IsWithinTimeFrame(OscTimeTag time);

    /// <summary>
    /// Get the difference in seconds between the current time and the suppied time
    /// </summary>
    /// <param name="time">the time to compair</param>
    /// <returns>the difference in seconds between the current time and the suppied time</returns>
    double DifferenceInSeconds(OscTimeTag time);
  }
}