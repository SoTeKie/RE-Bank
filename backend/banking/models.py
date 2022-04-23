import uuid

from django_countries.fields import CountryField
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
    country = CountryField()
    state = models.CharField(max_length=40, blank=True)
    street = models.CharField(max_length=500)
    additional_info = models.JSONField()

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


class BankAccount(BaseModel):
    user = models.ForeignKey(UserProfile, on_delete=models.PROTECT)
    balance = MoneyField(max_digits=14, decimal_places=2, default_currency="EUR")
    iban = models.CharField(max_length=34)


class Card(BaseModel):
    bank_account = models.ForeignKey(BankAccount, on_delete=models.PROTECT)


class Transaction(BaseModel):
    bank_account = models.ForeignKey(BankAccount, on_delete=models.PROTECT, null=True)
    card = models.ForeignKey(Card, on_delete=models.PROTECT, null=True)
    amount = MoneyField(max_digits=14, decimal_places=2, default_currency="EUR")
    # Only one of the parties is guaranteed to be on our site.
    # As a workaround we simply store data about the other party in an indexed JSON field.
    other_party = models.JSONField()

    @property
    def is_sender(self):
        return self.amount.amount < 0

    @property
    def is_receiver(self):
        return self.amount.amount > 0
