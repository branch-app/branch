package models

import "time"

type XboxLiveAuthentication struct {
	Token     string
	UserHash  string
	ExpiresAt time.Time
	Identity  *XboxLiveIdentity
}

type XboxLiveAuthResponse struct {
	AccessToken  string `json:"access_token"`
	RefreshToken string `json:"refresh_token"`
}

type XboxLiveAuthenticationRequest struct {
	Properties   *XboxLiveAuthenticationPropertiesRequest `json:"Properties"`
	RelyingParty string                                   `json:"RelyingParty"`
	TokenType    string                                   `json:"TokenType"`
}

type XboxLiveAuthenticationPropertiesRequest struct {
	AuthMethod string `json:"AuthMethod"`
	RpsTicket  string `json:"RpsTicket"`
	SiteName   string `json:"SiteName"`
}

type XboxLiveAuthenticationResponse struct {
	IssueInstant  time.Time                      `json:"IssueInstant"`
	NotAfter      time.Time                      `json:"NotAfter"`
	Token         string                         `json:"Token"`
	DisplayClaims map[string][]map[string]string `json:"DisplayClaims"`
}

type XboxLiveAuthorizationRequest struct {
	Properties   *XboxLiveAuthorizationPropertiesRequest `json:"Properties"`
	RelyingParty string                                  `json:"RelyingParty"`
	TokenType    string                                  `json:"TokenType"`
}

type XboxLiveAuthorizationPropertiesRequest struct {
	SandboxID  string   `json:"SandboxId"`
	UserTokens []string `json:"UserTokens"`
}

type XboxLiveAuthorizationResponse struct {
	XboxLiveAuthenticationResponse
}
