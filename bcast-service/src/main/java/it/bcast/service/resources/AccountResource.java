package it.bcast.service.resources;

import it.bcast.service.core.Account;
import it.bcast.service.bCastServiceConfiguration;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;

@Path("/accounts/{accountName}")
@Produces(MediaType.APPLICATION_JSON)
public class AccountResource {

    private bCastServiceConfiguration configuration;

    public AccountResource(bCastServiceConfiguration configuration) {
        this.configuration = configuration;
    }

    @GET
    public Account getAccount(@PathParam("accountName") String name) {
        return null;
    }
}