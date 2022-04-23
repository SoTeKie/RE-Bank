import uuid

from djmoney.models.fields import MoneyField
from django.db import models
from django.contrib.auth.models import User


class BaseModel(models.Model):
    id = models.UUIDField(primary_key=True, default=uuid.uuid4)
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)
    deleted_at = models.DateTimeField(null=True)

    class Meta:
        abstract = True


class Address(BaseModel):
    class Meta:
        abstract = True


class BillingAddress(Address):
    pass


class ShippingAddress(Address):
    pass


class ChildParent(BaseModel):
    pass


class UserProfile(BaseModel):
    class Roles(models.TextChoices):
        ADMIN = "admin"
        CLIENT = "client"
        RESTRICTED_CLIENT = "r_client"

    user = models.OneToOneField(User, on_delete=models.CASCADE)
    role = models.CharField(max_length=10, choices=Roles.choices, default=Roles.CLIENT)
    phone_number = models.CharField(max_length=15)
    is_verified = models.BooleanField(default=False)
    shipping_address = models.OneToOneField(ShippingAddress, on_delete=models.CASCADE)
    billing_address = models.OneToOneField(BillingAddress, on_delete=models.CASCADE)
    children = models.ManyToManyField(
        "self", related_name="parents", through=ChildParent
    )


class Transaction(BaseModel):
    pass


class BankAccount(BaseModel):
    user = models.ForeignKey(UserProfile, on_delete=models.PROTECT)
    balance = MoneyField(max_digits=14, decimal_places=2, default_currency="EUR")
    iban = models.CharField(max_length=34)


class Card(BaseModel):
    bank_account = models.ForeignKey(BankAccount, on_delete=models.PROTECT)
