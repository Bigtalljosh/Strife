output "strife_ip" {
    value = azurerm_container_group.strife-acg.ip_address
}

output "strife_dns" {
    value = azurerm_container_group.strife-acg.fqdn
}