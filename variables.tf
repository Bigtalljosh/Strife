variable "imagebuild" {
  type        = string
  description = "Latest Image Build"
}

variable "auth0clientid" {
  type        = string
  description = "Auth0 ClientID"
}

variable "auth0clientsecret" {
  type        = string
  description = "Auth0 ClientID"
}

variable "auth0domain" {
  type        = string
  description = "Auth0 Domain"
}

variable "auth0apiidentifier" {
  type        = string
  description = "Auth0 API Identifier"
}

variable "azureblobconnectionstring" {
  type        = string
  description = "Azure Blob Storage Connection String" 
}