package it.bcast.service.core;

import com.fasterxml.jackson.annotation.JsonProperty;
import org.hibernate.validator.constraints.Length;
import org.hibernate.validator.constraints.NotEmpty;
import java.time.LocalDateTime;


public class Cast {
    @NotEmpty
    @Length(max = 20)
    private String id;
    private Endpoint source;
    private String content;
    private String contentType;
    private LocalDateTime created;

    @JsonProperty
    public String getId() {
        return id;
    }
    @JsonProperty
    public void setId(String id) {
        this.id = id;
    }

    @JsonProperty
    public Endpoint getSource() {
        return source;
    }
    @JsonProperty
    public void setSource(Endpoint source) {
        this.source = source;
    }

    @JsonProperty
    public String getContent() {
        return content;
    }
    @JsonProperty
    public void setContent(String content) {
        this.content = content;
    }

    @JsonProperty
    public String getContentType() {
        return contentType;
    }
    @JsonProperty
    public void setContentType(String contentType) {
        this.contentType = contentType;
    }

    @JsonProperty
    public LocalDateTime getCreated() {
        return created;
    }
    @JsonProperty
    public void setCreated(LocalDateTime created) {
        this.created = created;
    }

}