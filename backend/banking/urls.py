from django.urls import include, path
from rest_framework import routers
from rest_framework.authtoken import views
from . import views as custom_views

router = routers.DefaultRouter()
router.register("transactions", custom_views.TransactionView, basename="transaction")
router.register(
    "shipping-addresses", custom_views.ShippingAddressView, basename="shipping_address"
)
router.register(
    "billing-addresses", custom_views.BillingAddressView, basename="billing_address"
)
router.register("cards", custom_views.CardView, basename="card")
router.register("bank-accounts", custom_views.BankAccountView, basename="bank_account")


# Wire up our API using automatic URL routing.
# Additionally, we include login URLs for the browsable API.
urlpatterns = [
    path("", include(router.urls)),
    path("me/", custom_views.current_user),
    path("login/", views.obtain_auth_token),
    path("register/", custom_views.register),
]
