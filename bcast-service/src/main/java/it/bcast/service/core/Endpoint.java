package it.bcast.service.core;

import com.fasterxml.jackson.annotation.JsonProperty;
import org.hibernate.validator.constraints.Length;
import org.hibernate.validator.constraints.NotEmpty;

public class Endpoint {
    @NotEmpty
    @Length(max = 20)
    private String id;
    private Account account;
    @NotEmpty
    @Length(max = 20)
    private String type;
    @Length(max = 20)
    private String subtype;
    private Boolean isActive;

    @JsonProperty
    public String getId() {
        return id;
    }
    @JsonProperty
    public void setId(String id) {
        this.id = id;
    }

    @JsonProperty
    public Account getAccount() {
        return account;
    }
    @JsonProperty
    public void setAccount(Account account) {
        this.account = account;
    }

    @JsonProperty
    public String getType() {
        return type;
    }
    @JsonProperty
    public void setType(String type) {
        this.type = type;
    }

    @JsonProperty
    public String getSubType() {
        return subtype;
    }
    @JsonProperty
    public void setSubType(String subtype) {
        this.subtype = subtype;
    }

    @JsonProperty
    public Boolean getIsActive() {
        return isActive;
    }
    @JsonProperty
    public void setIsActive(Boolean isActive) {
        this.isActive = isActive;
    }
}