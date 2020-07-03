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
  }
}