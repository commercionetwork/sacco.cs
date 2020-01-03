// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
//

using System;
using System.Diagnostics;

namespace commercio.sacco.lib
{
    // *** This is inherited by Equatable in Dart Package!
    //  There is no such Class in C# - we include Compare-Net-Objects Nuget package for the purpose - see https://github.com/GregFinzer/Compare-Net-Objects
    public class NodeInfo 
    {
        #region Properties

        public String network { get; private set; }

        #endregion

        #region Constructors

        public NodeInfo(String network)
        {
            Trace.Assert(network != null);
            this.network = network;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Helpers

        #endregion

    }
}
