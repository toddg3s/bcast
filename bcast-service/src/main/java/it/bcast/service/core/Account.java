package it.bcast.service.core;

import com.fasterxml.jackson.annotation.JsonProperty;
import org.hibernate.validator.constraints.Length;
import org.hibernate.validator.constraints.NotEmpty;

public class Account {
    private long id;
    @NotEmpty
    @Length(max = 50)
    private String name;
    private Contact owner;

    @JsonProperty
    public long getId() {
        return id;
    }
    @JsonProperty
    public void setId(long id) {
        this.id = id;
    }

    @JsonProperty
    public String getName() {
        return name;
    }
    @JsonProperty
    public void setName(String name) {
        this.name = name;
    }

    @JsonProperty
    public Contact getOwner() {
        return owner;
    }
    @JsonProperty
    public void setOwner(Contact owner) {
        this.owner = owner;
    }
    
}