﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using DatabaseEntities;
using TransferObjects;


namespace IServices
{
    [ServiceContract]
    public interface IServiceSprava
    {
        
        [OperationContract]
        [WebInvoke(Method = "GET",
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped,
           UriTemplate = "jedlo/{id_jedla}/{id_jazyka}")]
        TJedlo Jedlo(string id_jedla, string id_jazyka);

       /* [OperationContract]
        ICollection<TJedlo> vsetkyJedla(String startingWith, String id_jazyka);

        [OperationContract]
        IList<TTypJedla> typyJedal(String id_jazyka);

        [OperationContract]
        Boolean update_jedlo(String session,TJedlo jedlo);
        */
    }
}
