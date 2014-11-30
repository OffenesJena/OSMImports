/*
 * Copyright (c) 2014, Achim 'ahzf' Friedland <achim@graphdefined.org>
 * This file is part of OpenDataAPI <http://www.github.com/GraphDefined/OpenDataAPI>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

#endregion

namespace org.GraphDefined.OpenDataAPI.OverpassAPI
{

    /// <summary>
    /// The JSON result of an Overpass query.
    /// </summary>
    public static partial class OverpassAPIExtentions
    {

        #region RunNow(this ResultTask)

        /// <summary>
        /// Run the given Overpass query.
        /// </summary>
        /// <param name="ResultTask">A Overpass query result task.</param>
        public static void RunNow(this Task<OverpassResult> ResultTask)
        {
            ResultTask.ContinueWith(task => Console.WriteLine("ready!")).Wait();
        }

        #endregion

        #region RunNow(this JSONTask)

        /// <summary>
        /// Run the given Overpass query.
        /// </summary>
        /// <param name="JSONTask">A Overpass query result task.</param>
        public static void RunNow(this Task<JObject> JSONTask)
        {
            JSONTask.ContinueWith(task => Console.WriteLine("ready!")).Wait();
        }

        #endregion

    }

}
