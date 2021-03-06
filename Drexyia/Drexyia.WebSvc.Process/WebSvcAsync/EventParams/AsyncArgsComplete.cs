/*
    Copyright 2013 Fred Bowker
    This file is part of WsdlUI.
    WsdlUI is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
    WsdlUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along with Foobar. If not, see http://www.gnu.org/licenses/.
*/

using System;

namespace Drexyia.WebSvc.Process.WebSvcAsync.EventParams {
    public class AsyncArgsComplete : AsyncArgs {

        public DateTime StartTime {
            get;
            private set;
        }

        public DateTime EndTime {
            get;
            private set;
        }

        public int TotalTime {
            get {
                return (int)(EndTime - StartTime).TotalMilliseconds;
            }           
        }

        public AsyncArgsComplete(string name, DateTime startTime, DateTime endTime) 
            : base(name) {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
