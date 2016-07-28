package it.bcast.service;

import it.bcast.service.health.BasicHealthCheck;
import it.bcast.service.resources.AccountResource;
import it.bcast.service.resources.EndpointResource;
import it.bcast.service.resources.CastResource;
import com.yammer.dropwizard.Service;
import com.yammer.dropwizard.assets.AssetsBundle;
import com.yammer.dropwizard.config.Bootstrap;
import com.yammer.dropwizard.config.Environment;
import com.yammer.dropwizard.db.DatabaseConfiguration;

public class bCastService extends Service<bCastServiceConfiguration> {
    public static void main(String[] args) throws Exception {
        new bCastService().run(args);
    }

    @Override
    public void initialize(Bootstrap<bCastServiceConfiguration> bootstrap) {
        bootstrap.setName("bCast");
        bootstrap.addBundle(new AssetsBundle());
    }

    @Override
    public void run(bCastServiceConfiguration configuration,
                    Environment environment) throws ClassNotFoundException {

        environment.addHealthCheck(new BasicHealthCheck());
        environment.addResource(new AccountResource(configuration));
        environment.addResource(new EndpointResource(configuration));
        environment.addResource(new CastResource(configuration));
    }
}
