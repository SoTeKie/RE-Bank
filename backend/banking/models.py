import uuid

from django_countries.fields import CountryField
from djmoney.models.fields import MoneyField
from django.db import models
from django.contrib.auth.models import User


class BaseModel(models.Model):
    id = models.UUIDField(primary_key=True, default=uuid.uuid4)
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)
    deleted_at = models.DateTimeField(null=True, blank=True)

    class Meta:
        abstract = True


class Address(BaseModel):
    country = CountryField()
    state = models.CharField(max_length=40, blank=True)
    city = models.CharField(max_length=85)
    street = models.CharField(max_length=500)
    additional_info = models.JSONField(null=True, blank=True)

    class Meta:
        abstract = True


class BillingAddress(Address):
    user = models.OneToOneField(
        "UserProfile",
        on_delete=models.CASCADE,
        related_name="billing_address",
    )


class ShippingAddress(Address):
    user = models.OneToOneField(
        "UserProfile",
        on_delete=models.CASCADE,
        related_name="shipping_address",
    )


class ChildParent(BaseModel):
    child = models.ForeignKey(
        "UserProfile", on_delete=models.PROTECT, related_name="parent_child"
    )
    parent = models.ForeignKey(
        "UserProfile", on_delete=models.PROTECT, related_name="child_parent"
    )


class UserProfile(BaseModel):
    class Role(models.TextChoices):
        ADMIN = "admin"
        CLIENT = "client"
        RESTRICTED_CLIENT = "r_client"

    user = models.OneToOneField(User, on_delete=models.CASCADE)
    middle_name = models.CharField(max_length=50, blank=True)
    date_of_birth = models.DateField()
    title = models.CharField(max_length=10, blank=True)
    role = models.CharField(max_length=10, choices=Role.choices, default=Role.CLIENT)
    phone_number = models.CharField(max_length=15)
    is_verified = models.BooleanField(default=False)

    children = models.ManyToManyField(
        "self", through=ChildParent, symmetrical=False, related_name="parents"
    )

    @property
    def full_name(self):
        middle_name = f" {self.middle_name}" if self.middle_name != "" else ""
        return f"{self.user.first_name}{middle_name} {self.user.last_name}"

    @property
    def formatted_name(self):
        middle_name = f" {self.middle_name[0]}." if self.middle_name != "" else ""
        return f"{self.title} {self.user.first_name}{middle_name} {self.user.last_name}"

    @property
    def base_profile(self):
        return {
            "id": self.id,
            "email": self.user.email,
            "title": self.title,
            "first_name": self.user.first_name,
            "middle_name": self.middle_name,
            "last_name": self.user.last_name,
            "full_name": self.full_name,
            "formatted_name": self.formatted_name,
            "phone": self.phone_number,
            "role": self.role,
        }

    @property
    def is_child(self):
        return self.role == self.Role.RESTRICTED_CLIENT

    @property
    def __full_parent_profile(self):
        return {
            **self.base_profile,
            "children": [child.base_profile for child in self.children.all()],
        }

    @property
    def __full_child_profile(self):
        return {
            **self.base_profile,
            "children": [parent.base_profile for parent in self.parents.all()],
        }

    @property
    def full_profile(self):
        return (
            self.__full_child_profile if self.is_child else self.__full_parent_profile
        )


class BankAccount(BaseModel):
    class Type(models.TextChoices):
        DEFAULT = "default"
        SAVINGS = "savings"

    user = models.ForeignKey(UserProfile, on_delete=models.PROTECT)
    balance = MoneyField(max_digits=14, decimal_places=2, default_currency="EUR")
    iban = models.CharField(max_length=34)
    type = models.CharField(max_length=10, choices=Type.choices)
    is_active = models.BooleanField(default=False)


class Card(BaseModel):
    class Brand(models.TextChoices):
        VISA = "visa"
        MASTERCARD = "mastercard"
        AMEX = "amex"
        OTHER = "other"

    bank_account = models.ForeignKey(BankAccount, on_delete=models.PROTECT)
    pan_last_4 = models.PositiveSmallIntegerField()
    # This field technically doesn't have to match the users name for formatting reasons
    # ex.: O'Reilly
    cardholder_name = models.CharField(max_length=100)
    expiration_date = models.CharField(max_length=5)
    brand = models.CharField(max_length=10, choices=Brand.choices)
    is_active = models.BooleanField(default=False)


class Transaction(BaseModel):
    """This class describes all payments either recieved or sent from an account on our platform"""

    bank_account = models.ForeignKey(BankAccount, on_delete=models.PROTECT)
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
