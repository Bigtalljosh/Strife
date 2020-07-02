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

resource "azurerm_resource_group" "strife-rg" {
    name     = "rg-strife"
    location = "UK South"
}

resource "azurerm_app_service_plan" "strife-asp" {
    name                = "asp-uks-strife"
    location            = azurerm_resource_group.strife-rg.location
    resource_group_name = azurerm_resource_group.strife-rg.name
    kind                = "Linux"
    reserved            = true

    sku {
        tier = "Standard"
        size = "S1"
    }
}

resource "azurerm_app_service" "strife-app" {
    name                = "as-strife"
    location            = azurerm_resource_group.strife-rg.location
    resource_group_name = azurerm_resource_group.strife-rg.name
    app_service_plan_id = azurerm_app_service_plan.strife-asp.id

    site_config {
        dotnet_framework_version = "v4.0"
        http2_enabled            = true
    }

    app_settings = {
        
    }
}