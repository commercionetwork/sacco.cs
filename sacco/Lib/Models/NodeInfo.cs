// 
// sacco - Base Library for Commercio Network
//
// Riccardo Costacurta
// Dec. 2, 2019
// BlockIt s.r.l.
// 
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace sacco.Lib
{
    // *** This is inherited by Equatable in Dart Package!
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


        /*
        class NodeInfo extends Equatable {
         final String network;

         NodeInfo({
           @required this.network,
         })  : assert(network != null),
               super([network]);
       }

        */
    }
}
