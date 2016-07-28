package it.bcast.service.resources;

import it.bcast.service.core.Account;
import it.bcast.service.core.Endpoint;
import it.bcast.service.bCastServiceConfiguration;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;

@Path("/accounts/{accountName}/endpoints/{endpointName}")
@Produces(MediaType.APPLICATION_JSON)
public class EndpointResource {

    private bCastServiceConfiguration configuration;

    public EndpointResource(bCastServiceConfiguration configuration) {
        this.configuration = configuration;
    }

    @GET
    public Endpoint getEndpoint(@PathParam("accountName") String accountName, @PathParam("endpointName") String endpointName) {
        return null;
    }
}