package it.bcast.service.health;

import com.google.common.base.Optional;
import com.yammer.metrics.core.HealthCheck;

public class BasicHealthCheck extends HealthCheck {

    public BasicHealthCheck() {
        super("basic");
    }
    @Override
    protected Result check() throws Exception {
        return Result.healthy();
    }
}