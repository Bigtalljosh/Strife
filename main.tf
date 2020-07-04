provider "azurerm" {
    version = "2.5.0"
    features {}
}

terraform {
    backend "azurerm" {
        resource_group_name     = "rg-terraform"
        storage_account_name    = "btjterraform"
        container_name          = "tfstate"
        key                     = "terraform.tfstate"
    }
}

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

resource "azurerm_resource_group" "strife-rg" {
    name     = "rg-strife"
    location = "UK South"
}

resource "azurerm_container_group" "tfcg_test" {
  name                      = "strife"
  location                  = azurerm_resource_group.strife-rg.location
  resource_group_name       = azurerm_resource_group.strife-rg.name

  ip_address_type     = "public"
  os_type             = "Linux"

  container {
      name            = "strife"
      image           = "bigtalljosh/strife:${var.imagebuild}"
        cpu             = "1"
        memory          = "1"

        ports {
            port        = 5000
            protocol    = "TCP"
        }

        secure_environment_variables = {
            ASPNETCORE_Auth0__ClientId          = var.auth0clientid
            ASPNETCORE_Auth0__ClientSecret      = var.auth0clientsecret
            ASPNETCORE_Auth0__Domain            = var.auth0domain
            ASPNETCORE_Auth0__ApiIdentifier     = var.auth0apiidentifier
        }
  }
}