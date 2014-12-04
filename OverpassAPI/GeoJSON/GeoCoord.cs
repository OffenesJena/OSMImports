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
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.OpenDataAPI.OverpassAPI
{

    /// <summary>
    /// A geo coordinate.
    /// </summary>
    public struct GeoCoord : IEquatable<GeoCoord>
    {

        #region Data

        /// <summary>
        /// A geo longitude.
        /// </summary>
        public readonly Double Longitude;

        /// <summary>
        /// A geo latitude.
        /// </summary>
        public readonly Double Latitude;

        #endregion

        #region GeoCoord(Longitude, Latitude)

        /// <summary>
        /// Create a new geo coordinate.
        /// </summary>
        /// <param name="_Longitude">A geo longitude.</param>
        /// <param name="_Latitude">A geo latitude.</param>
        public GeoCoord(Double _Longitude,
                        Double _Latitude)
        {
            Longitude = _Longitude;
            Latitude = _Latitude;
        }

        #endregion


        #region Operator overloading

        #region Operator == (GeoCoord1, GeoCoord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GeoCoord1">A GeoCoord.</param>
        /// <param name="GeoCoord2">Another GeoCoord.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (GeoCoord GeoCoord1, GeoCoord GeoCoord2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GeoCoord1, GeoCoord2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GeoCoord1 == null) || ((Object) GeoCoord2 == null))
                return false;

            return GeoCoord1.Equals(GeoCoord2);

        }

        #endregion

        #region Operator != (GeoCoord1, GeoCoord2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="GeoCoord1">A GeoCoord.</param>
        /// <param name="GeoCoord2">Another GeoCoord.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (GeoCoord GeoCoord1, GeoCoord GeoCoord2)
        {
            return !(GeoCoord1 == GeoCoord2);
        }

        #endregion

        #endregion

        #region IEquatable<RevId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            if (!(Object is GeoCoord))
                return false;

            var _Object = (GeoCoord) Object;

            if (Longitude != _Object.Longitude)
                return false;

            if (Latitude  != _Object.Latitude)
                return false;

            return true;

        }

        #endregion

        #region Equals(GeoCoord)

        /// <summary>
        /// Compares two GeoCoords for equality.
        /// </summary>
        /// <param name="GeoCoord">A GeoCoord to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(GeoCoord GeoCoord)
        {

            if ((Object) GeoCoord == null)
                return false;

            if (Latitude  != GeoCoord.Latitude)
                return false;

            if (Longitude != GeoCoord.Longitude)
                return false;

            return true;

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            return Longitude.GetHashCode() ^ Latitude.GetHashCode();
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Returns a formated string representation of this object.
        /// </summary>
        /// <returns>A formated string representation of this object.</returns>
        public override String ToString()
        {
            return Longitude.ToString() + ", " + Latitude.ToString();
        }

        #endregion

    }

}
