package it.bcast.service.resources;

import it.bcast.service.core.Account;
import it.bcast.service.core.Endpoint;
import it.bcast.service.core.Cast;
import it.bcast.service.bCastServiceConfiguration;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;

@Path("/cast/{castId}")
@Produces(MediaType.APPLICATION_JSON)
public class CastResource {

    private bCastServiceConfiguration configuration;

    public CastResource(bCastServiceConfiguration configuration) {
        this.configuration = configuration;
    }

    @GET
    public Endpoint getCast(@PathParam("castId") String castId) {
        return null;
    }
}