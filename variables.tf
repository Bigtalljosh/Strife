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

variable "azurecosmosdatabasename" {
  type        = string
  description = "Azure CosmosDb Database Name" 
}

variable "azurecosmosprimarykey" {
  type        = string
  description = "Azure CosmosDb Primary key" 
}

variable "azurecosmosuri" {
  type        = string
  description = "Azure CosmosDb Uri" 
}