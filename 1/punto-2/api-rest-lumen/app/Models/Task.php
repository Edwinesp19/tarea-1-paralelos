<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Task extends Model
{
    //

    public function status()
    {
        return $this->belongsTo(TaskStatus::class,'status_id','id');
    }
}
